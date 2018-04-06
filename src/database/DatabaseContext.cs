using System;
using Microsoft.EntityFrameworkCore;
using database.Models;
using System.Linq;

namespace HomeSchooling
{
    public class Repository : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CoursesListener> CoursesListeners { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        public Repository()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=HomeSchooling;integrated security=True;MultipleActiveResultSets=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Lesson>().ToTable("Lessons");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<CoursesListener>().ToTable("CoursesListeners");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Attachment>().ToTable("Attachments");
        }
    }

    class DatabaseContext
    {
        static void Main(string[] args)
        {
            using (Repository db = new Repository())
            {
                //db.Lessons.Add(new Lesson("Modest", DateTime.Now));
                //db.Lessons.Add(new Lesson("ROman", DateTime.Now));
                //db.SaveChanges();
                IQueryable<Lesson>  allLessons = from elem in db.Lessons
                                                 select elem;
                foreach(var elem in allLessons)
                {
                    Console.WriteLine("{0,-10}{1}", elem.Id, elem.Name);
                }
            }
            Console.ReadKey();
        }
    }
}
