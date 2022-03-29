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
        public DbSet<Record> Records { get; set; }

        public LastWeekContext(DbContextOptions<LastWeekContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            modelBuilder.Entity<Record>().HasKey(r => r.Guid);

            modelBuilder.Entity<ChoiceRecord>().Property(e => e.Choices).HasConversion(splitListStringConverter);
            modelBuilder.Entity<SimpleRecord>().Property(e => e.Answers).HasConversion(splitListStringConverter);
            modelBuilder.Entity<RangeRecord>().Property(e => e.Boundaries).HasConversion(rangeConverter);
            modelBuilder.Entity<TextRecord>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
