using System.Threading.Tasks;
using Tools;

namespace LastWeek.Exporter.Interfaces
{
    public interface IFileSaver
    {
        bool LocalSaver { get; }
        Result SaveFile(byte[] file, string name, string mimeType, string description, string folder, params object[] additionalParams);
        Task<Result> SaveFileAsync(byte[] file, string name, string mimeType, string description, string folder, params object[] additionalParams);

    }
}
