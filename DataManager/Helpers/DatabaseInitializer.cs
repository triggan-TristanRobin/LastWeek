using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataManager.Helpers
{
    public static class DatabaseInitializer
    {
        public static void Initialize(ReviewDatabaseContext context)
        {
#if DEBUG
            context.Database.EnsureDeleted();
#endif
            context.Database.EnsureCreated();

            if (!context.Reviews.Any())
            {
                var reviews = new List<Review>();
                var date = DateTime.Today;
                reviews.Add(new Review
                {
                    StartDate = date,
                    EndDate = date.AddDays(1),
                    Status = ReviewStatus.Active,
                    Entries = new List<Entry>
                {
                    //new ChoiceEntry() { Choices = new List<string> { "Choice1", "Choice2", "Choice3"}, Question = "First question?"},
                    new RangeEntry() { Boundaries = 1..10, Question = "Second question?"},
                    new SimpleEntry() { Answers = new List<string>() { string.Empty }, Question = "Third question?"},
                    new TextEntry() { Answer = string.Empty, Question = "Next question?"},
                    new RangeEntry() { Boundaries = 1..10, Question = "Second question?"},
                    new SimpleEntry() { Answers = new List<string>() { string.Empty }, Question = "Third question?"},
                    new TextEntry() { Answer = string.Empty, Question = "Next question?"},
                    new RangeEntry() { Boundaries = 1..10, Question = "Second question?"},
                    new SimpleEntry() { Answers = new List<string>() { string.Empty }, Question = "Third question?"},
                    new TextEntry() { Answer = string.Empty, Question = "Next question?"}
                }
                });

                for (int i = 1; i < 10; i++)
                {
                    date = date.AddDays(-1);
                    reviews.Add(new Review
                    {
                        StartDate = date,
                        EndDate = date.AddDays(1),
                        Entries = new List<Entry>
                        {
                            //new ChoiceEntry() { Choices = new List<string> { "Choice1", "Choice2", "Choice3"}, Question = "First question?"},
                            new RangeEntry() { Boundaries = 1..10, Question = "Second question?"},
                            new SimpleEntry() { Answers = new List<string>() { string.Empty }, Question = "Third question?"},
                            new TextEntry() { Answer = string.Empty, Question = "Next question?"}
                        }
                    });
                }

                context.AddRange(reviews);
                context.SaveChanges();
            }
        }
    }
}
