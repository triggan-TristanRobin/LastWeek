using Microsoft.EntityFrameworkCore;
using LastWeek.Model;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Linq;

namespace DataManager.Context
{
    public class SQLiteContext : DbContext
    {
        /// <summary>
        /// Manipulate the posts table
        /// </summary>
        /// <value>The property that allows to access the Posts table</value>
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<ChoiceEntry> ChoiceEntries { get; set; }
        public DbSet<RangeEntry> RangeEntries { get; set; }
        public DbSet<SimpleEntry> SimpleEntries { get; set; }
        public DbSet<TextEntry> TextEntries { get; set; }

        private static string databasePath;

        public static void SetDatabasePath(string path)
        {
            databasePath = Path.Combine(path, "database.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>().HasKey(r => r.Guid);
            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            modelBuilder.Entity<ChoiceEntry>().Property(e => e.Choices).HasConversion(splitStringConverter);
            var splitListStringConverter = new ValueConverter<List<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }).ToList());
            modelBuilder.Entity<SimpleEntry>().Property(e => e.Answers).HasConversion(splitListStringConverter);
            var rangeConverter = new ValueConverter<Range, string>(r => $"{r.Start};{r.End}", s => new Range(int.Parse(s.Split(new[] { ';' })[0]), int.Parse(s.Split(new[] { ';' })[1])));
            modelBuilder.Entity<RangeEntry>().Property(e => e.Boundaries).HasConversion(rangeConverter);
            base.OnModelCreating(modelBuilder);
        }
    }
}
