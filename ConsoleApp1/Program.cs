using DataManager;
using DataManager.Helpers;
using Model;
using System;
using System.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var dbManager = new LocalDBManager<ReviewDatabaseContext>();
            var reviews = dbManager.GetReviewsAsync().Result;
            var review = reviews.First();
            var entry = review.Entries[0] as ChoiceEntry;
            Console.WriteLine($"I found {reviews.Count} reviews!");
        }
    }
}
