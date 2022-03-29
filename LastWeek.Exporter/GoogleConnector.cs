using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Drive.v3.Data;
using LastWeek.Exporter.Interfaces;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace LastWeek.Exporter
{
	public class GoogleConnector : IAccountConnector
	{
		// If modifying these scopes, delete your previously saved credentials
		// at ~/.credentials/drive-dotnet-quickstart.json
		static string[] Scopes = { DriveService.Scope.Drive };
		static string ApplicationName = "LiReS Descktop Classic Reviewer";
		public User User { get; set; }
		public DriveService Service { get; set; }
		public bool LoggedIn => User != null;

		private readonly ILogger<GoogleConnector> logger;

        public GoogleConnector(ILogger<GoogleConnector> logger)
        {
			this.logger = logger;
        }

        public bool Login()
		{
			UserCredential credential;

            logger.LogInformation("Google login");
			using (var stream = new FileStream(@"./Resources/client_id.json", FileMode.Open, FileAccess.Read))
            {
                logger.LogInformation("Found client id file");
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				credPath = Path.Combine(credPath, ".credentials/LiReS.json");

				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.FromStream(stream).Secrets,
					Scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
				// TODO : Log Console.WriteLine("Credential file saved to: " + credPath);
			}

			// Create Drive API service.
			Service = new DriveService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

            logger.LogInformation("Retrieving info from authorization result");
			var getRequest = Service.About.Get();
			getRequest.Fields = "user";
			return (User = getRequest.Execute().User) != null;
		}
	}
}