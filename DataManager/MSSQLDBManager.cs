using LastWeek.Model;
using System;
using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataManager.Helpers;
using System.Linq;

namespace DataManager
{
    public class MSSQLDBManager : IAsyncContentManager, IDisposable
    {
        private readonly LastWeekContext context;

        public MSSQLDBManager(LastWeekContext lastWeekContext)
        {
            context = lastWeekContext;
            DatabaseInitializer.Initialize(context);
        }

        public async Task<List<Review>> GetReviewsAsync(int count = 0)
        {
            return await context.Reviews
                                .AsNoTracking().Include(r => r.Entries)
                                .OrderByDescending(review => review.StartDate)
                                .ToListAsync();
        }

        public async Task<Review> GetReviewAsync(Guid guid)
        {
            return await context.Reviews.Include(r => r.Entries).Where(r => r.Guid == guid).FirstOrDefaultAsync();
        }

        private async Task<bool> AnyAsync(Review review)
        {
            return await context.Reviews.CountAsync(r => r.Guid == review.Guid) > 0;
        }

        private async Task<int> PostReviewAsync(Review reviewToSave)
        {
            await context.Reviews.AddAsync(reviewToSave);
            return await context.SaveChangesAsync();
        }

        private async Task<int> PutReviewAsync(Review reviewToUpdate)
        {
            context.Entry(reviewToUpdate).State = EntityState.Modified;
            foreach (var entry in reviewToUpdate.Entries)
            {
                context.Entry(entry).State = EntityState.Modified;
            }
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

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
