using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.IO.Abstractions;

namespace DataManager
{
    public class ContentManager : IContentManager
    {
        private readonly string filePath;
        private readonly IFileSystem fileSystem;

        public ContentManager() : this(string.Empty, new FileSystem()) { }

        public ContentManager(string filePath, IFileSystem fileSystem)
        {
            this.filePath = filePath;
            this.fileSystem = fileSystem;
        }

        public List<Review> GetReviews(int count = 0)
        {
            var reviews = new List<Review>();
            if (fileSystem.File.Exists(filePath))
            {
                var fileStr = fileSystem.File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(fileStr))
                {
                    reviews = JsonSerializer.Deserialize<List<Review>>(fileStr);
                }
            }
            return reviews;
        }

        public Review GetReview(Guid guid)
        {
            Review review = null;
            if (fileSystem.File.Exists(filePath))
            {
                var fileStr = fileSystem.File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(fileStr))
                {
                    var reviews = JsonSerializer.Deserialize<IEnumerable<Review>>(fileStr);
                    review = reviews.Single(r => r.Guid == guid);
                }
            }
            return review;
        }

        private bool Any(Review review)
        {
            return GetReviews().Any(r => r.Guid == review.Guid);
        }

        private void PostReview(Review reviewToSave)
        {
            var reviews = GetReviews();
            reviews.Add(reviewToSave);
            var jsonReview = JsonSerializer.Serialize<IEnumerable<Review>>(reviews);
            fileSystem.File.WriteAllText(filePath, jsonReview);
        }

        private void PutReview(Review reviewToUpdate)
        {
            var reviews = GetReviews();
            reviews.Single(r => r.Guid == reviewToUpdate.Guid).Update(reviewToUpdate);
            var jsonReview = JsonSerializer.Serialize<IEnumerable<Review>>(reviews);
            fileSystem.File.WriteAllText(filePath, jsonReview);
        }

        public void UpsertReview(Review reviewToSave)
        {
            if (Any(reviewToSave))
            {
                PutReview(reviewToSave);
            }
            else
            {
                PostReview(reviewToSave);
            }
        }
    }
}
