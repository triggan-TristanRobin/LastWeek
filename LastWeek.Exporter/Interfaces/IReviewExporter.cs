using LastWeek.Model;
using Tools;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ReviewExporter.Interfaces
{
    public interface IReviewExporter
    {
        void WriteDoc(Review review);
        bool GetFile();
        Task<Result> StoreReviewAsync(string path, List<object> additionalParams);
    }
}
