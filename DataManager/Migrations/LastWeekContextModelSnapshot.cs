﻿// <auto-generated />
using System;
using DataManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataManager.Migrations
{
    [DbContext(typeof(LastWeekContext))]
    partial class LastWeekContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LastWeek.Model.Entry", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ReviewGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Guid");

                    b.HasIndex("ReviewGuid");

                    b.ToTable("Entries");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Entry");
                });

            modelBuilder.Entity("LastWeek.Model.Review", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Guid");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("LastWeek.Model.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Guid");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LastWeek.Model.ChoiceEntry", b =>
                {
                    b.HasBaseType("LastWeek.Model.Entry");

                    b.Property<string>("Choices")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Selected")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ChoiceEntry");
                });

            modelBuilder.Entity("LastWeek.Model.RangeEntry", b =>
                {
                    b.HasBaseType("LastWeek.Model.Entry");

                    b.Property<string>("Boundaries")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Selected")
                        .HasColumnType("float")
                        .HasColumnName("RangeEntry_Selected");

                    b.HasDiscriminator().HasValue("RangeEntry");
                });

            modelBuilder.Entity("LastWeek.Model.SimpleEntry", b =>
                {
                    b.HasBaseType("LastWeek.Model.Entry");

                    b.Property<string>("Answers")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("SimpleEntry");
                });

            modelBuilder.Entity("LastWeek.Model.Entry", b =>
                {
                    b.HasOne("LastWeek.Model.Review", null)
                        .WithMany("Entries")
                        .HasForeignKey("ReviewGuid");
                });

            modelBuilder.Entity("LastWeek.Model.Review", b =>
                {
                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
