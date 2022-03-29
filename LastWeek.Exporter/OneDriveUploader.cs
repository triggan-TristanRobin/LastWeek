using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using LastWeek.Exporter.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Tools;

namespace LastWeek.Exporter
{
    public class OneDriveUploader : IFileSaver
    {
        private const string AADClientId = "9c03d7c4-de27-4478-a829-f1f4363bca0c";
        private const string GraphAPIEndpointPrefix = "https://graph.microsoft.com/v1.0/";
        private static string Tenant = "common";
        private string[] AADScopes = { "files.readwrite.all" };
        private IPublicClientApplication AADAppContext = null;
        private GraphServiceClient graphClient = null;

        public AuthenticationResult UserCredentials { get; set; }

        public bool LocalSaver { get { return false; } }

        private readonly ILogger<OneDriveUploader> logger;

        public OneDriveUploader(ILogger<OneDriveUploader> logger)
        {
            this.logger = logger;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public void InitializeGraph()
        {
            if (UserCredentials != null)
            {
                graphClient = new GraphServiceClient(
                    GraphAPIEndpointPrefix,
                    new DelegateAuthenticationProvider(
                        async (requestMessage) =>
                        {
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", UserCredentials.AccessToken);
                        }
                    )
                );
            }
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary> 
        /// Log the user in to either Office 365 or OneDrive consumer 
        /// </summary> 
        /// <returns>A task to await on</returns> 
        public async Task<bool> SignInUser(object parentWindow = null)
        {
            logger.LogInformation("Sign in");
            var result = false;

            // Instantiate the app with AAD 
            AADAppContext = PublicClientApplicationBuilder.Create(AADClientId)
                                                            .WithRedirectUri("http://localhost:7111")
                                                            .Build();

            string status;
            try
            {
                try
                {
                    var accounts = await AADAppContext.GetAccountsAsync();
                    UserCredentials = await AADAppContext.AcquireTokenSilent(AADScopes, accounts.FirstOrDefault())
                                                              .ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    UserCredentials = await AADAppContext.AcquireTokenInteractive(AADScopes)
                        .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
                        .ExecuteAsync();
                }
                if (UserCredentials != null)
                {
                    status = "Signed in as " + UserCredentials.Account.Username;
                    logger.LogInformation($"{status}");
                    InitializeGraph();
                    result = true;
                }
            }

            catch (MsalServiceException serviceEx)
            {
                status = $"Could not sign in, error code: " + serviceEx.ErrorCode;
                logger.LogError($"{status}, {serviceEx.Message}");
            }

            catch (Exception ex)
            {
                status = $"Error Acquiring Token: {ex}";
                logger.LogError($"{status}, {ex.Message}");
            }

            return result;
        }

        /// <summary> 
        /// Take a file and upload it to the service 
        /// </summary> 
        /// <param name="fileToUpload">The file that we want to upload</param> 
        /// <param name="uploadToSharePoint">Should we upload to SharePoint or OneDrive?</param> 
        public async Task UploadSmallFile(string fileName, Stream fileStreamToUpload, bool uploadToSharePoint = false)
        {
            logger.LogInformation("Uploading file");
            DriveItem uploadedFile;
            var driveRoot = uploadToSharePoint ? graphClient.Sites["root"].Drive.Root : graphClient.Me.Drive.Root;
            // Do we want OneDrive for Business/Consumer or do we want a SharePoint Site? 
            uploadedFile = await driveRoot.ItemWithPath(fileName).Content.Request().PutAsync<DriveItem>(fileStreamToUpload);
        }

        public Result SaveFile(byte[] file, string name, string mimeType, string description, string folder, params object[] additionalParams)
        {
            var stream = new MemoryStream(file);
            var result = false;

            logger.LogInformation("Saving file");
            try
            {
                var signInTask = Task.Run(async () => await SignInUser(additionalParams.Any() ? additionalParams[0] : null));

                if (signInTask.Result)
                {
                    var uploadTask = Task.Run(async () => await UploadSmallFile($"{folder}/{name}", stream));
                    result = true;
                }
            }
            catch (Exception e)
            {
                logger.LogError($"An error occurred, {e.Message}");
                result = false;
            }
            return new Result { Success = result, Message = "Review " + (result ? "saved" : "failed to save") + " on OneDrive" };
        }

        public async Task<Result> SaveFileAsync(byte[] file, string name, string mimeType, string description, string folder, params object[] additionalParams)
        {
            var stream = new MemoryStream(file);
            var result = false;

            logger.LogInformation("Saving file async");
            try
            {
                var isSignedIn = await SignInUser(additionalParams.Any() ? additionalParams[0] : null);

                if (isSignedIn)
                {
                    await UploadSmallFile($"{folder}/{name}", stream);
                    result = true;
                }
            }
            catch (Exception e)
            {
                logger.LogError($"An error occurred, {e.Message}");
                result = false;
            }
            return new Result { Success = result, Message = "Review " + (result ? "saved" : "failed to save") + " on OneDrive" };
        }
    }
}
