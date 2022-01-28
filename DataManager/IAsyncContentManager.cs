using LastWeek.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManager
{
    public interface IAsyncContentManager
    {
        Task<List<Review>> GetReviewsAsync(int count = 0);
        Task<Review> GetReviewAsync(Guid guid);
        Task<int> UpsertReviewAsync(Review reviewToSave);
    }
}