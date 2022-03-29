using LastWeek.Model;
using System;
using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataManager.Helpers;
using System.Linq;
using DataManager.Context;

namespace DataManager
{
    public class LocalDBManager : IAsyncContentManager
    {
        private readonly SQLiteContext context;
        private SQLiteContext GetContext()
        {
            var context = this.context ?? new SQLiteContext();
            return context;
        }

        public LocalDBManager(string localDBPath = "")
        {
            localDBPath = string.IsNullOrEmpty(localDBPath) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : localDBPath;
            SQLiteContext.SetDatabasePath(localDBPath);
            DatabaseInitializer.Initialize(GetContext());
        }

        public LocalDBManager(SQLiteContext context)
        {
            this.context = context;
            DatabaseInitializer.Initialize(this.context);
        }

        public async Task<List<Review>> GetReviewsAsync(int count = 0, Guid? userId = null)
        {
            using var context = GetContext();
            return await context.Reviews
                                .AsNoTracking().Include(r => r.Records)
                                .OrderByDescending(review => review.StartDate)
                                .ToListAsync();
        }

        public async Task<Review> GetReviewAsync(Guid guid, Guid? userId = null)
        {
            using var context = GetContext();
            return await context.Reviews.Include(r => r.Records).Where(r => r.Guid == guid).FirstOrDefaultAsync();
        }

        private async Task<bool> AnyAsync(Review review)
        {
            using var context = GetContext();
            return await context.Reviews.CountAsync(r => r.Guid == review.Guid) > 0;
        }

        private async Task<int> PostReviewAsync(Review reviewToSave)
        {
            using var context = GetContext();
            await context.Reviews.AddAsync(reviewToSave);
            return await context.SaveChangesAsync();
        }

        private async Task<int> PutReviewAsync(Review reviewToUpdate)
        {
            using var context = GetContext();
            context.Entry(reviewToUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpsertReviewAsync(Review reviewToSave, Guid? userId = null)
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
