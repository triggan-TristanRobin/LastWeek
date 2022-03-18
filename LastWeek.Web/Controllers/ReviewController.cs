using DataManager;
using Microsoft.AspNetCore.Mvc;
using LastWeek.Model;

namespace LastWeek.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IAsyncContentManager contentManager;

        public ReviewController(ILogger<ReviewController> logger, IAsyncContentManager contentManager)
        {
            _logger = logger;
            this.contentManager = contentManager;
        }

        [HttpGet(Name = "Reviews")]
        public async Task<JsonResult> GetReviewAsync(Guid? guid, int? count = 0)
        {
            if(guid != null)
            {
                var review = await contentManager.GetReviewAsync(guid.Value);
                var res = new JsonResult(review);
                // TODO: add type inheritence serialization
                return res;
            }
            else
            {
                var reviews = await contentManager.GetReviewsAsync(count ?? 0);
                return new JsonResult(reviews);
            }
        }

        [HttpPost(Name = "Reviews")]
        public async Task<JsonResult> UpsertReviewAsync(Review reviewToSave)
        {
            var result = await contentManager.UpsertReviewAsync(reviewToSave);
            return new JsonResult(result);
        }
    }
}