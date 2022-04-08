﻿// <auto-generated />
using System;
using DataManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataManager.Migrations
{
    [DbContext(typeof(LastWeekContext))]
    [Migration("20220408070519_AddRecordOrder")]
    partial class AddRecordOrder
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LastWeek.Model.Record", b =>
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

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ReviewGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Guid");

                    b.HasIndex("ReviewGuid");

                    b.ToTable("Records");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Record");
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

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("UserGuid");

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

            modelBuilder.Entity("LastWeek.Model.ChoiceRecord", b =>
                {
                    b.HasBaseType("LastWeek.Model.Record");

                    b.Property<string>("Choices")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Selected")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ChoiceRecord_Selected");

                    b.HasDiscriminator().HasValue("ChoiceRecord");
                });

            modelBuilder.Entity("LastWeek.Model.RangeRecord", b =>
                {
                    b.HasBaseType("LastWeek.Model.Record");

                    b.Property<string>("Boundaries")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Selected")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("RangeRecord");
                });

            modelBuilder.Entity("LastWeek.Model.SimpleRecord", b =>
                {
                    b.HasBaseType("LastWeek.Model.Record");

                    b.Property<string>("Answers")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("SimpleRecord");
                });

            modelBuilder.Entity("LastWeek.Model.TextRecord", b =>
                {
                    b.HasBaseType("LastWeek.Model.Record");

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("TextRecord");
                });

            modelBuilder.Entity("LastWeek.Model.Record", b =>
                {
                    b.HasOne("LastWeek.Model.Review", null)
                        .WithMany("Records")
                        .HasForeignKey("ReviewGuid");
                });

            modelBuilder.Entity("LastWeek.Model.Review", b =>
                {
                    b.HasOne("LastWeek.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserGuid");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LastWeek.Model.Review", b =>
                {
                    b.Navigation("Records");
                });
#pragma warning restore 612, 618
        }
    }
}
