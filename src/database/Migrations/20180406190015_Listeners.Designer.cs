﻿// <auto-generated />
using HomeSchooling;
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
    [Migration("20180406190015_Listeners")]
    partial class Listeners
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("database.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("TeacherId");

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

                    b.Property<int?>("StudentId");

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

                    b.Property<DateTime>("Date");

                    b.Property<string>("HomeWorkDescription");

                    b.Property<bool>("IsControlWork");

                    b.Property<string>("LessonDescription");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("database.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<string>("UserRole")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
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
#pragma warning restore 612, 618
        }
    }
}
