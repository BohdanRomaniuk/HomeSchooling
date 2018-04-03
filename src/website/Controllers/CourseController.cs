using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using Microsoft.AspNetCore.Http;

namespace website.Controllers
{
    public class CourseController : Controller
    {
        private readonly HomeSchoolingContext db;

        public CourseController(HomeSchoolingContext _db)
        {
            db = _db;
        }
        
        public IActionResult ViewCourse(int id)
        {
            return View(db.Courses.Include(c=>c.Teacher).Include(c=>c.CourseLessons).Where(o=>o.Id==id).SingleOrDefault());
        }

        public IActionResult ViewLesson(int id)
        {
            return View(db.Lessons.Where(l=>l.Id==id).SingleOrDefault());
        }

        public IActionResult RequestCourse(int id)
        {
            if(HttpContext.Session.GetInt32("role") !=null)
            {
                string role = HttpContext.Session.GetString("role");
                return View("RequestCourse",role);
            }
            return View(HttpContext.Session.GetInt32("role"));
        }
    }
}