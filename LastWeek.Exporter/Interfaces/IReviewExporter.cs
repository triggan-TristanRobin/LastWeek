using LastWeek.Model;
using Tools;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace LastWeek.Exporter.Interfaces
{
    public interface IReviewExporter
    {
        byte[] WriteDoc(Review review);
        bool GetFile();
        Task<Result> StoreReviewAsync(string path, List<object> additionalParams);
    }
}
