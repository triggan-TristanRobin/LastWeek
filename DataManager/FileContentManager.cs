using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace DataManager
{
    public class FileContentManager : IAsyncContentManager
    {
        private readonly string filePath;
        private readonly IFileSystem fileSystem;

        public FileContentManager() : this(string.Empty, new FileSystem()) { }

        public FileContentManager(string filePath, IFileSystem fileSystem)
        {
            this.filePath = filePath;
            this.fileSystem = fileSystem;
        }

        public async Task<List<Review>> GetReviewsAsync(int count = 0)
        {
            var reviews = new List<Review>();
            if (fileSystem.File.Exists(filePath))
            {
                var fileStr = await fileSystem.File.ReadAllTextAsync(filePath);
                if (!string.IsNullOrEmpty(fileStr))
                {
                    reviews = JsonSerializer.Deserialize<List<Review>>(fileStr);
                }
            }
            return reviews;
        }

        public async Task<Review> GetReviewAsync(Guid guid)
        {
            Review review = null;
            if (fileSystem.File.Exists(filePath))
            {
                var fileStr = await fileSystem.File.ReadAllTextAsync(filePath);
                if (!string.IsNullOrEmpty(fileStr))
                {
                    var reviews = JsonSerializer.Deserialize<IEnumerable<Review>>(fileStr);
                    review = reviews.Single(r => r.Guid == guid);
                }
            }
            return review;
        }

        private async Task<bool> AnyAsync(Review review)
        {
            return (await GetReviewsAsync()).Any(r => r.Guid == review.Guid);
        }

        private async Task<int> PostReviewAsync(Review reviewToSave)
        {
            var reviews = await GetReviewsAsync();
            reviews.Add(reviewToSave);
            var jsonReview = JsonSerializer.Serialize<IEnumerable<Review>>(reviews);
            fileSystem.File.WriteAllText(filePath, jsonReview);
            return 1;
        }

        private async Task<int> PutReviewAsync(Review reviewToUpdate)
        {
            var reviews = await GetReviewsAsync();
            reviews.Single(r => r.Guid == reviewToUpdate.Guid).Update(reviewToUpdate);
            var jsonReview = JsonSerializer.Serialize<IEnumerable<Review>>(reviews);
            fileSystem.File.WriteAllText(filePath, jsonReview);
            return 1;
        }

        public async Task<int> UpsertReviewAsync(Review reviewToSave)
        {
            if (await AnyAsync(reviewToSave))
            {
                return await PutReviewAsync(reviewToSave);
            }
            else
            {
                return await PostReviewAsync(reviewToSave);
            }
        }
    }
}
