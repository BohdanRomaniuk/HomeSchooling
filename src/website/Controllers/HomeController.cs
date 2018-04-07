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
        private readonly HomeSchoolingContext _context;

        public HomeController(HomeSchoolingContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page=1)
        {
            IQueryable<Course> allCourses = _context.Courses.Include(o => o.Teacher);
            return View(allCourses);
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
