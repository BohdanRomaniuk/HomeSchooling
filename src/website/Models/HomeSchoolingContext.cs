using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using database.Models;

namespace website.Models
{
    public class HomeSchoolingContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<CoursesListener> CoursesListeners { get; set; }

        public HomeSchoolingContext(DbContextOptions<HomeSchoolingContext> options):
            base(options)
        {
        }
    }
}
