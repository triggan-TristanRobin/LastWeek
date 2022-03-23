using DataManager;
using Microsoft.AspNetCore.Mvc;
using LastWeek.Model;
using System.Text.Json;
using DataManager.Helpers;

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
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new EntryConverter());
            if (guid != null)
            {
                var review = await contentManager.GetReviewAsync(guid.Value);
                return new JsonResult(review, serializeOptions);
            }
            else
            {
                var reviews = await contentManager.GetReviewsAsync(count ?? 0);
                return new JsonResult(reviews, serializeOptions);
            }
        }

        [HttpPost(Name = "Reviews")]
        public async Task<IActionResult> UpsertReviewAsync(Review review)
        {
            var guid = Guid.NewGuid();
            review.Guid = review.Guid == new Guid() ? guid : review.Guid;
            var result = await contentManager.UpsertReviewAsync(review);

            if(result >= 0)
            {
                var serializeOptions = new JsonSerializerOptions();
                serializeOptions.Converters.Add(new EntryConverter());
                var savedReview = await contentManager.GetReviewAsync(guid);
                return new JsonResult(savedReview, serializeOptions);
            }
            return StatusCode(500);
        }
    }
}