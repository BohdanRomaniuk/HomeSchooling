using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using database.Models;
namespace website.Controllers
{
    public class AdminController : Controller
    {
        private IHomeSchoolingRepository db;
        public AdminController(IHomeSchoolingRepository _db)
        {
            db = _db;
        }

        public IActionResult SetTeacher(int id)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "admin")
                {
                    //User teacher = db.Users.Where(u => u.Id == id).SingleOrDefault();
                    //teacher.UserRole = "teacher";
                    //db.SaveChanges();
                    db.SetTeacher(id);
                }
            }
            return RedirectToRoute(new { controller = "Profile", action = "View", id = id });
        }

        public IActionResult DeleteCourse(int id)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "admin")
                {

                    //Course course = db.Courses.Include(c => c.CourseLessons).ThenInclude(l => l.Posts).Where(c=>c.Id == id).SingleOrDefault();
                    //var listeners = db.CoursesListeners.Where(c => c.RequestedCourse.Id == id);
                    //foreach (var l in listeners)
                    //{
                    //    db.CoursesListeners.Remove(l);
                    //}
                    //foreach (var l in course.CourseLessons)
                    //{
                    //    foreach (var p in l.Posts)
                    //    {
                    //        db.Posts.Remove(p);
                    //    }
                    //    db.Lessons.Remove(l);
                    //}
                    //db.Remove(course);
                    //db.SaveChanges();
                    db.DeleteCourse(id);
                }
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}