using LastWeek.Model;
using LastWeek.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    public class FakeContentManager : IAsyncContentManager
    {

        public async Task<Review> GetReviewAsync(Guid guid, Guid? userId = null)
        {
            var date = DateTime.Today;
            await Task.Delay(100);
            return new Review
            {
                StartDate = date,
                EndDate = date.AddDays(1),
                Status = ReviewStatus.Active,
                Records = new List<Record>
                {
                    new ChoiceRecord() { Choices = new List<string> { "Choice1", "Choice2", "Choice3"}, Question = "First question?"},
                    new RangeRecord() { Boundaries = 1..10, Question = "Second question?"},
                    new SimpleRecord() { Answers = new List<string>() { string.Empty }, Question = "Third question?"},
                    new TextRecord() { Answer = string.Empty, Question = "Next question?"}
                }
            };
        }

        public async Task<List<Review>> GetReviewsAsync(int count = 0, Guid? userId = null)
        {

            var reviews = new List<Review>();
            var date = DateTime.Today;
            reviews.Add(new Review
            {
                StartDate = date,
                EndDate = date.AddDays(1),
                Status = ReviewStatus.Active,
                Records = new List<Record>
                {
                    new ChoiceRecord() { Choices = new List<string> { "Choice1", "Choice2", "Choice3"}, Question = "First question?"},
                    new RangeRecord() { Boundaries = 1..10, Question = "Second question?"},
                    new SimpleRecord() { Answers = new List<string>() { string.Empty }, Question = "Third question?"},
                    new TextRecord() { Answer = string.Empty, Question = "Next question?"}
                }
            });

            for (int i = 1; i < (count == 0 ? 50 : count); i++)
            {
                date = date.AddDays(-1);
                reviews.Add(new Review { StartDate = date, EndDate = date.AddDays(1) });
            }
            await Task.Delay(100);
            return reviews;
        }

        public async Task<int> UpsertReviewAsync(Review reviewToSave, Guid? userId = null)
        {
            await Task.Delay(100);
            return 1;
        }
        public async Task<bool> DeleteReviewAsync(Review reviewToSave, Guid? userId = null)
        {
            await Task.Delay(100);
            return true;
        }
    }
}
