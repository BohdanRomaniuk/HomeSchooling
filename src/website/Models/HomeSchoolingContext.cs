using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace website.Models
{
    public class HomeSchoolingContext: IdentityDbContext<User>
    {
        //public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<CoursesListener> CoursesListeners { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Mark> Marks { get; set; }

        public HomeSchoolingContext(DbContextOptions<HomeSchoolingContext> options):
            base(options)
        {
        }
    }
}
