using Microsoft.EntityFrameworkCore;
using LastWeek.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using DataManager.Context;

namespace DataManager.Helpers
{
    public static class DatabaseInitializer
    {
        public static void Initialize(SQLiteContext context)
        {
#if DEBUG
            context.Database.EnsureDeleted();
#endif
            context.Database.EnsureCreated();

            DbSet<Review> reviewDbSet = context.Reviews;
            InitializeReviews(context, reviewDbSet);
        }

        public static void Initialize(LastWeekContext context)
        {
            context.Database.Migrate();

            DbSet<Review> reviewDbSet = context.Reviews;
            InitializeReviews(context, reviewDbSet);
        }

        private static void InitializeReviews(DbContext context, DbSet<Review> reviewDbSet)
        {
            if (!reviewDbSet.Any())
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
