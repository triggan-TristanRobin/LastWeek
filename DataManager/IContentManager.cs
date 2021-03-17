using Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManager
{
    public interface IContentManager
    {
        List<Review> GetReviews(int count = 0);
    }
}