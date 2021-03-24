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

        /*public async Task<bool> PostEntityAsync<T>(string Slug, T entity) where T : Entity
        {
            var success = await Http.PostAsJsonAsync($"Commit/{typeof(T).Name}/{Slug}", entity);

            return success.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProjectAsync(string slug, Update update)
        {
            var savedProject = await GetEntityAsync<Project>(slug);
            savedProject.Updates.Add(update);
            return await PostEntityAsync(slug, savedProject);
        }

        public async Task<int> StarEntity<T>(string slug) where T : Entity
        {
            var entity = await GetEntityAsync<T>(slug);
            entity.Stars++;
            var success = await FunctionsHttp.GetAsync(Settings.GetFullUrl(typeof(T).Name, entity.Slug, "Star", local: false));

            Console.WriteLine("Star result: " + await success.Content.ReadAsStringAsync());
            return int.Parse(await success.Content.ReadAsStringAsync());
        }*/
    }
}
