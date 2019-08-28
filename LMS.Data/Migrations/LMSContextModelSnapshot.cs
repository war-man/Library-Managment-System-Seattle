﻿// <auto-generated />
using System;
using LMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LMS.Data.Migrations
{
    [DbContext(typeof(LMSContext))]
    partial class LMSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LMS.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("LMS.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId");

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<int>("IsbnId");

                    b.Property<string>("Language")
                        .IsRequired();

                    b.Property<int>("Pages");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("IsbnId")
                        .IsUnique();

                    b.ToTable("Books");
                });

            modelBuilder.Entity("LMS.Models.BookSubject", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("SubjectCategoryId");

                    b.HasKey("BookId", "SubjectCategoryId");

                    b.HasIndex("SubjectCategoryId");

                    b.ToTable("BooksSubjects");
                });

            modelBuilder.Entity("LMS.Models.HistoryRegistry", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("UserId");

                    b.HasKey("BookId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("HistoryRegistries");
                });

            modelBuilder.Entity("LMS.Models.Models.Isbn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ISBN");

                    b.HasKey("Id");

                    b.ToTable("Isbns");
                });

            modelBuilder.Entity("LMS.Models.Models.ReserveBook", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("UserId");

                    b.Property<DateTime?>("ReservationDate");

                    b.HasKey("BookId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ReserveBook");
                });

            modelBuilder.Entity("LMS.Models.RecordFines", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("FineAmount");

                    b.HasKey("Id");

                    b.ToTable("RecordFines");
                });

            modelBuilder.Entity("LMS.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("LMS.Models.SubjectCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SubjectName");

                    b.HasKey("Id");

                    b.ToTable("SubjectCategories");
                });

            modelBuilder.Entity("LMS.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("RecordFinesId");

                    b.Property<int>("RoleId");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RecordFinesId")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LMS.Models.Book", b =>
                {
                    b.HasOne("LMS.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LMS.Models.Models.Isbn", "Isbn")
                        .WithOne("Book")
                        .HasForeignKey("LMS.Models.Book", "IsbnId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LMS.Models.BookSubject", b =>
                {
                    b.HasOne("LMS.Models.Book", "Book")
                        .WithMany("BookSubject")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LMS.Models.SubjectCategory", "SubjectCategory")
                        .WithMany("BookSubject")
                        .HasForeignKey("SubjectCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LMS.Models.HistoryRegistry", b =>
                {
                    b.HasOne("LMS.Models.Book", "Book")
                        .WithMany("HistoryRegistries")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LMS.Models.User", "User")
                        .WithMany("HistoryRegistries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LMS.Models.Models.ReserveBook", b =>
                {
                    b.HasOne("LMS.Models.Book", "Book")
                        .WithMany("ReservedBooks")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LMS.Models.User", "User")
                        .WithMany("ReservedBooks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LMS.Models.User", b =>
                {
                    b.HasOne("LMS.Models.RecordFines", "RecordFines")
                        .WithOne("User")
                        .HasForeignKey("LMS.Models.User", "RecordFinesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LMS.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
