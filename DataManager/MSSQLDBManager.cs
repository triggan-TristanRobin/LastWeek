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
                query = query.Where(review => review.User != null && review.User.Guid == userId && !review.Deleted);
            return await query.AsNoTracking().Include(r => r.Records)
                                .OrderByDescending(review => review.StartDate)
                                .ToListAsync();
        }

        public async Task<Review> GetReviewAsync(Guid guid, Guid? userId = null)
        {
            var query = context.Reviews.AsQueryable();
            if (userId != null)
                query = query.Where(review => review.User != null && review.User.Guid == userId);
            return await query.Include(r => r.Records).Where(r => r.Guid == guid).FirstOrDefaultAsync();
        }

        private async Task<bool> AnyAsync(Review review)
        {
            return review.Guid == new Guid() || await context.Reviews.CountAsync(r => r.Guid == review.Guid) > 0;
        }

        private async Task<int> PostReviewAsync(Review reviewToSave, Guid? userId = null)
        {
            if (reviewToSave.Guid == new Guid()) reviewToSave.Guid = Guid.NewGuid();
            if (userId != null) reviewToSave.User = context.Users.SingleOrDefault(u => u.Guid == userId);

            reviewToSave.Records?.ForEach(e => e.Guid = e.Guid == new Guid() ? Guid.NewGuid() : e.Guid);
            await context.Reviews.AddAsync(reviewToSave);
            return await context.SaveChangesAsync();
        }

        private async Task<int> PutReviewAsync(Review reviewToUpdate, Guid? userId = null)
        {
            context.Entry(reviewToUpdate).State = EntityState.Modified;
            var dbReview = await context.Reviews.Include(r => r.Records).Include(r => r.User).AsNoTracking().Where(r => r.Guid == reviewToUpdate.Guid).FirstOrDefaultAsync();
            if (dbReview.User.Guid != userId) throw new Exception("Cannot update review which does not belong to the user");
            
            foreach (var record in dbReview.Records)
            {
                var recordToUdpate = reviewToUpdate.Records.SingleOrDefault(e => e.Guid == record.Guid);
                if (recordToUdpate != null)
                {
                    record.Update(recordToUdpate);
                    context.Entry(record).State = EntityState.Modified;
                }
                else
                    context.Entry(record).State = EntityState.Deleted;
            }
            foreach (var record in reviewToUpdate.Records.Where(e => !dbReview.Records.Any(dbE => dbE.Guid == e.Guid)))
            {
                context.Entry(record).State = EntityState.Added;
            }
            dbReview.Update(reviewToUpdate);
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

        public async Task<bool> DeleteReviewAsync(Review reviewToSave, Guid? userId = null)
        {
            if (await AnyAsync(reviewToSave))
            {
                if (context.Entry(reviewToSave).State == EntityState.Detached)
                    reviewToSave = await GetReviewAsync(reviewToSave.Guid, userId);

                reviewToSave.Deleted = true;
                return await context.SaveChangesAsync() == 1;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
