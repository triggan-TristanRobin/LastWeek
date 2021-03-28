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

        public  List<Review> GetReviews(int count = 0)
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

        public bool PostReview(Review reviewToSave)
        {
            var reviews = GetReviews();
            if (!reviews.Any(r => r.Guid == reviewToSave.Guid))
            {
                reviews.Add(reviewToSave);
                var jsonReview = JsonSerializer.Serialize<IEnumerable<Review>>(reviews);
                fileSystem.File.WriteAllText(filePath, jsonReview);
                return true;
            }
            return false;
        }

        public bool PutReview(Review reviewToUpdate)
        {
            if (fileSystem.File.Exists(filePath))
            {
                var reviews = GetReviews();
                if (reviews.Any(r => r.Guid == reviewToUpdate.Guid))
                {
                    reviews.Single(r => r.Guid == reviewToUpdate.Guid).Update(reviewToUpdate);
                    var jsonReview = JsonSerializer.Serialize<IEnumerable<Review>>(reviews);
                    fileSystem.File.WriteAllText(filePath, jsonReview);
                    return true;
                }
                return false;
            }
            throw new FileNotFoundException();
        }
    }
}
