using Model;
using System;
using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataManager.Helpers;
using System.Linq;

namespace DataManager
{
    public class LocalDBManager<T> : IAsyncContentManager
    {
        private readonly ReviewDatabaseContext context;
        private ReviewDatabaseContext GetContext()
        {
            var context = this.context ?? new ReviewDatabaseContext();
            return context;
        }

        public LocalDBManager(string localDBPath = "")
        {
            localDBPath = string.IsNullOrEmpty(localDBPath) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : localDBPath;
            ReviewDatabaseContext.SetDatabasePath(localDBPath);
            DatabaseInitializer.Initialize(GetContext());
        }

        public LocalDBManager(ReviewDatabaseContext context)
        {
            this.context = context;
            DatabaseInitializer.Initialize(this.context);
        }

        public async Task<List<Review>> GetReviewsAsync(int count = 0)
        {
            using var context = GetContext();
            return await context.Reviews
                                .AsNoTracking().Include(r => r.Entries)
                                .OrderByDescending(review => review.StartDate)
                                .ToListAsync();
        }

        public async Task<Review> GetReviewAsync(Guid guid)
        {
            using var context = GetContext();
            return await context.Reviews.Include(r => r.Entries).Where(r => r.Guid == guid).FirstOrDefaultAsync();
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
