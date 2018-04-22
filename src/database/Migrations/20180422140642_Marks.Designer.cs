﻿// <auto-generated />
using database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace database.Migrations
{
    [DbContext(typeof(Repository))]
    [Migration("20180422140642_Marks")]
    partial class Marks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("database.Models.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.Property<int?>("PostId");

                    b.Property<DateTime>("UploadDate");

                    b.Property<string>("UploadedById");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UploadedById");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("database.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("TeacherId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("database.Models.CoursesListener", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Accepted");

                    b.Property<int?>("RequestedCourseId");

                    b.Property<string>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("RequestedCourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("CoursesListeners");
                });

            modelBuilder.Entity("database.Models.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CourseId");

                    b.Property<DateTime>("HomeWorkEnd");

                    b.Property<bool>("IsControlWork");

                    b.Property<DateTime>("LessonEndDate");

                    b.Property<DateTime>("LessonStartDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("database.Models.Mark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("MarkDate");

                    b.Property<int>("MarkValue");

                    b.Property<int?>("MarkedLessonId");

                    b.Property<int?>("Post");

                    b.Property<string>("StudentId");

                    b.Property<string>("TeacherId");

                    b.HasKey("Id");

                    b.HasIndex("MarkedLessonId");

                    b.HasIndex("Post")
                        .IsUnique()
                        .HasFilter("[Post] IS NOT NULL");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Marks");
                });

            modelBuilder.Entity("database.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LessonId");

                    b.Property<string>("PostType")
                        .IsRequired();

                    b.Property<string>("PostedById");

                    b.Property<DateTime>("PostedDate");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.HasIndex("PostedById");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("database.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("database.Models.Attachment", b =>
                {
                    b.HasOne("database.Models.Post")
                        .WithMany("PostAtachments")
                        .HasForeignKey("PostId");

                    b.HasOne("database.Models.User", "UploadedBy")
                        .WithMany()
                        .HasForeignKey("UploadedById");
                });

            modelBuilder.Entity("database.Models.Course", b =>
                {
                    b.HasOne("database.Models.User", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("database.Models.CoursesListener", b =>
                {
                    b.HasOne("database.Models.Course", "RequestedCourse")
                        .WithMany()
                        .HasForeignKey("RequestedCourseId");

                    b.HasOne("database.Models.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("database.Models.Lesson", b =>
                {
                    b.HasOne("database.Models.Course")
                        .WithMany("CourseLessons")
                        .HasForeignKey("CourseId");
                });

            modelBuilder.Entity("database.Models.Mark", b =>
                {
                    b.HasOne("database.Models.Lesson", "MarkedLesson")
                        .WithMany()
                        .HasForeignKey("MarkedLessonId");

                    b.HasOne("database.Models.Post", "PostId")
                        .WithOne("PostMark")
                        .HasForeignKey("database.Models.Mark", "Post");

                    b.HasOne("database.Models.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");

                    b.HasOne("database.Models.User", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId");
                });

            modelBuilder.Entity("database.Models.Post", b =>
                {
                    b.HasOne("database.Models.Lesson")
                        .WithMany("Posts")
                        .HasForeignKey("LessonId");

                    b.HasOne("database.Models.User", "PostedBy")
                        .WithMany()
                        .HasForeignKey("PostedById");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("database.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("database.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("database.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("database.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
