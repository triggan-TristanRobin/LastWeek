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
    public class UserManager : IDisposable
    {
        private readonly LastWeekContext context;

    public UserManager(LastWeekContext lastWeekContext)
    {
        context = lastWeekContext;
        DatabaseInitializer.Initialize(context);
        }

        public async Task<List<User>> GetUsersAsync(int count = 0)
        {
            return await context.Users
                                .AsNoTracking()
                                .OrderByDescending(user => user.FullName)
                                .ToListAsync();
        }

        public IQueryable<User> GetUsers()
        {
            return context.Users.AsQueryable<User>();
        }

        public async Task<User> GetUserAsync(Guid guid)
    {
        return await context.Users.Where(r => r.Guid == guid).FirstOrDefaultAsync();
    }

    private async Task<bool> AnyAsync(User user)
    {
        return await context.Users.CountAsync(r => r.Guid == user.Guid) > 0;
    }

    private async Task<int> PostUserAsync(User userToSave)
    {
        await context.Users.AddAsync(userToSave);
        return await context.SaveChangesAsync();
    }

    private async Task<int> PutUserAsync(User userToUpdate)
    {
        context.Entry(userToUpdate).State = EntityState.Modified;
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpsertUserAsync(User userToSave)
    {
        if (await AnyAsync(userToSave))
        {
            return await PutUserAsync(userToSave);
        }
        else
        {
            return await PostUserAsync(userToSave);
        }
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
}
