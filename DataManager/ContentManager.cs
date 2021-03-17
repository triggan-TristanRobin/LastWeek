using Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DataManager
{
    public class ContentManager : IContentManager
    {
        private readonly string filePath;

        public ContentManager(string filePath)
        {
            this.filePath = filePath;
        }

        public  List<Review> GetReviews(int count = 0)
        {
            var reviews = new List<Review>();
            if (File.Exists(filePath))
            {
                var fileStr = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(fileStr))
                {
                    reviews = JsonSerializer.Deserialize<List<Review>>(fileStr);
                }
            }
            return reviews;
        }

        /*public async Task<T> GetEntityAsync<T>(string slug) where T : Entity
        {
            return (await Http.GetFromJsonAsync<IEnumerable<T>>(Settings.GetFullUrl(typeof(T).Name))).SingleOrDefault(e => e.Slug == slug);
        }

        public async Task<bool> PostEntityAsync<T>(string Slug, T entity) where T : Entity
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
