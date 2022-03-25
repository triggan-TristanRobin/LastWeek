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

        public async Task<List<Review>> GetReviewsAsync(int count = 0, Guid? userId = null)
        {
            var query = context.Reviews.AsQueryable();
            if (userId != null)
                query = query.Where(review => review.User != null && review.User.Guid == userId);
            return await query.AsNoTracking().Include(r => r.Entries)
                                .OrderByDescending(review => review.StartDate)
                                .ToListAsync();
        }

        public async Task<Review> GetReviewAsync(Guid guid, Guid? userId = null)
        {
            var query = context.Reviews.AsQueryable();
            if (userId != null)
                query = query.Where(review => review.User != null && review.User.Guid == userId);
            return await query.Include(r => r.Entries).Where(r => r.Guid == guid).FirstOrDefaultAsync();
        }

        private async Task<bool> AnyAsync(Review review)
        {
            return review.Guid == new Guid() || await context.Reviews.CountAsync(r => r.Guid == review.Guid) > 0;
        }

        private async Task<int> PostReviewAsync(Review reviewToSave, Guid? userId = null)
        {
            if (reviewToSave.Guid == new Guid()) reviewToSave.Guid = Guid.NewGuid();
            if (userId != null) reviewToSave.User = context.Users.SingleOrDefault(u => u.Guid == userId);

            reviewToSave.Entries?.ForEach(e => e.Guid = e.Guid == new Guid() ? Guid.NewGuid() : e.Guid);
            await context.Reviews.AddAsync(reviewToSave);
            return await context.SaveChangesAsync();
        }

        private async Task<int> PutReviewAsync(Review reviewToUpdate, Guid? userId = null)
        {
            context.Entry(reviewToUpdate).State = EntityState.Modified;
            var dbReview = await context.Reviews.Include(r => r.Entries).Include(r => r.User).AsNoTracking().Where(r => r.Guid == reviewToUpdate.Guid).FirstOrDefaultAsync();
            if (dbReview.User.Guid != userId) throw new Exception("Cannot update review which does not belong to the user");
            
            foreach (var entry in dbReview.Entries)
            {
                if(reviewToUpdate.Entries.Any(e => e.Guid == entry.Guid))
                    context.Entry(entry).State = EntityState.Modified;
                else
                    context.Entry(entry).State = EntityState.Deleted;
            }
            foreach (var entry in reviewToUpdate.Entries.Where(e => !dbReview.Entries.Any(dbE => dbE.Guid == e.Guid)))
            {
                context.Entry(entry).State = EntityState.Added;
            }
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpsertReviewAsync(Review reviewToSave, Guid? userId = null)
        {
            if (await AnyAsync(reviewToSave))
            {
                return await PutReviewAsync(reviewToSave, userId);
            }
            else
            {
                return await PostReviewAsync(reviewToSave, userId);
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
