using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using website.Models;
using database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeSchoolingRepository _context;

        public HomeController(IHomeSchoolingRepository context)
        {
            _context = context;
        }

        public IActionResult Index(string name = null)
        {
            if (name == null)
            {
                IQueryable<Course> allCourses = _context.Courses.Include(c => c.Teacher).Include(c => c.CourseLessons);
                return View(allCourses);
            }
            else
            {
                return View(_context.Courses.Include(c => c.Teacher).Include(c => c.CourseLessons).Where(s => s.Name.Contains(name)));
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
