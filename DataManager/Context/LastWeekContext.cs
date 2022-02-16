using LastWeek.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataManager
{
    public class LastWeekContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Entry> Entries { get; set; }

        public LastWeekContext(DbContextOptions<LastWeekContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join("%%", v), v => v.Split("%%", StringSplitOptions.RemoveEmptyEntries));
            var splitListStringConverter = new ValueConverter<List<string>, string>(v => string.Join("%%", v), v => v.Split("%%", StringSplitOptions.RemoveEmptyEntries).ToList());
            var rangeConverter = new ValueConverter<Range, string>(r => $"{r.Start}%%{r.End}", s
                => new Range(int.Parse(s.Split("%%", StringSplitOptions.RemoveEmptyEntries)[0]),
                    int.Parse(s.Split("%%", StringSplitOptions.RemoveEmptyEntries)[1])));

            modelBuilder.Entity<User>().HasKey(u => u.Guid);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().Ignore(u => u.Token);
            modelBuilder.Entity<User>().Ignore(u => u.FullName);
            modelBuilder.Entity<Review>().HasKey(r => r.Guid);
            modelBuilder.Entity<Entry>().HasKey(r => r.Guid);

            modelBuilder.Entity<ChoiceEntry>().Property(e => e.Choices).HasConversion(splitStringConverter);
            modelBuilder.Entity<SimpleEntry>().Property(e => e.Answers).HasConversion(splitListStringConverter);
            modelBuilder.Entity<RangeEntry>().Property(e => e.Boundaries).HasConversion(rangeConverter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
