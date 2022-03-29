using Microsoft.Extensions.Logging;
using LastWeek.Exporter.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using Tools;

namespace LastWeek.Exporter
{
    public class WinLocalFileSaver : IFileSaver
    {
        public bool LocalSaver { get { return true; } }
        private readonly ILogger<WinLocalFileSaver> logger;

        public WinLocalFileSaver(ILogger<WinLocalFileSaver> logger)
        {
            this.logger = logger;
        }

        public Result SaveFile(byte[] file, string name, string mimeType, string description, string destinationPath, params object[] additionalParams)
        {
            logger.LogInformation("File saving");
            var result = true;
            try
            {
                File.WriteAllBytes(destinationPath, file);
            }
            catch(Exception ex)
            {
                logger.LogError($"An error occured, {ex.Message}");
                result = false;
            }
            return new Result { Success = result, Message = "Review " + (result ? "saved" : "failed to save") + " locally" };
        }

        public Task<Result> SaveFileAsync(byte[] file, string name, string mimeType, string description, string folder, params object[] additionalParams)
        {
            logger.LogError("Tried async saving with local saver");
            return null;
        }
    }
}
