using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using Microsoft.Extensions.Logging;
using ReviewExporter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tools;

namespace ReviewExporter
{
    public class GoogleUploader : IFileSaver
    {
        public bool LocalSaver { get { return false; } }
        public GoogleConnector Connector { get; set; }

        private readonly ILogger<GoogleUploader> logger;

        public GoogleUploader(ILogger<GoogleUploader> logger, IAccountConnector accountConnector)
        {
            this.logger = logger;
            Connector = accountConnector as GoogleConnector;
            if (Connector == null) throw new Exception("Cannot use Google uploader with a non Google connector.");
        }

        public Result SaveFile(byte[] file, string name, string mimeType, string description, string folder, params object[] additionalParams)
        {
            logger.LogInformation("Saving file");
            File fileMeta = new File()
            {
                Name = name,
                Description = description,
                MimeType = mimeType,
            };

            try
            {
                var getRequest = Connector.Service.Files.List();
                getRequest.Fields = "files(id, name)";
                getRequest.Q = $"name = '{folder}'";
                var store = getRequest.Execute();
                string folderId = null;
                if (!store.Files.Any())
                    folderId = CreateDirectory($"{folder}", "LiReS review storage folder", "").Id;
                fileMeta.Parents = new List<string>() { folderId ?? store.Files.First().Id };
            }
            catch (Exception e)
            {
                logger.LogError("An error occurred: " + e.Message);
                return new Result { Success = false, Message = "Review failed to upload on drive" };
            }

            IUploadProgress result;

            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.MemoryStream(file))
            {
                try
                {
                    request = Connector.Service.Files.Create(fileMeta, stream, mimeType);
                    request.Fields = "id";
                    result = request.Upload();
                }
                catch (Exception e)
                {
                    logger.LogError("An error occurred: " + e.Message);
                    return new Result { Success = false, Message = "Review failed to upload on google drive" };
                }
            }
            return new Result { Success = result.Status == UploadStatus.Completed, Message = "Review " + (result.Status == UploadStatus.Completed ? "uploaded" : "failed to upload") + " on google drive" };
        }

        private File CreateDirectory(string name, string description, string parent)
        {
            File NewDirectory = null;

            logger.LogInformation($"New google drive directory creation ({name})");
            // Create metaData for a new Directory
            File file = new File()
            {
                Name = name,
                Description = description,
                MimeType = "application/vnd.google-apps.folder"
            };
            try
            {
                FilesResource.CreateRequest request = Connector.Service.Files.Create(file);
                NewDirectory = request.Execute();
            }
            catch (Exception e)
            {
                logger.LogError("An error occurred: " + e.Message);
            }

            return NewDirectory;
        }

        public Task<Result> SaveFileAsync(byte[] file, string name, string mimeType, string description, string folder, params object[] additionalParams)
        {
            logger.LogError("Tried async saving with google drive uploader");
            return null;
        }
    }
}
