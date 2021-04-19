using Model;
using System;
using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;

namespace DataManager
{
    public class LocalDBtManager : IAsyncContentManager
    {
        private readonly SQLiteAsyncConnection database;
        private readonly Func<Task> dbChanged;

        public LocalDBtManager(string dbPath, Func<Task> dbChanged)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Review>().Wait();
            this.dbChanged = dbChanged ?? throw new ArgumentNullException(nameof(dbChanged));
        }

        public LocalDBtManager(SQLiteAsyncConnection database, Func<Task> dbChanged)
        {
            this.database = database;
            this.dbChanged = dbChanged ?? throw new ArgumentNullException(nameof(dbChanged));
        }

        public async Task<List<Review>> GetReviewsAsync(int count = 0)
        {
            return await database.Table<Review>().ToListAsync();
        }

        public async Task<Review> GetReviewAsync(Guid guid)
        {
            return await database.Table<Review>().Where(r => r.Guid == guid).FirstOrDefaultAsync();
        }

        private async Task<bool> AnyAsync(Review review)
        {
            return await database.Table<Review>().CountAsync(r => r.Guid == review.Guid) > 0;
        }

        private async Task<int> PostReviewAsync(Review reviewToSave)
        {
            var result = await database.InsertAsync(reviewToSave);
            await dbChanged();
            return result;
        }

        private async Task<int> PutReviewAsync(Review reviewToUpdate)
        {
            var result = await database.UpdateAsync(reviewToUpdate);
            await dbChanged();
            return result;
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
