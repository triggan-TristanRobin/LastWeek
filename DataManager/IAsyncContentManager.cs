using LastWeek.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManager
{
    public interface IAsyncContentManager
    {
        Task<List<Review>> GetReviewsAsync(int count = 0, Guid? userId = null);
        Task<Review> GetReviewAsync(Guid guid, Guid? userId = null);
        Task<int> UpsertReviewAsync(Review reviewToSave, Guid? userId = null);
    }
}