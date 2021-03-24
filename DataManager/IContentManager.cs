using Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManager
{
    public interface IContentManager
    {
        List<Review> GetReviews(int count = 0);
        Review GetReview(Guid guid);
        bool PostReview(Review reviewToSave);
    }
}