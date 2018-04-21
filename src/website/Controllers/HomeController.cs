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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace website.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;
        private readonly IHomeSchoolingRepository _context;

        public HomeController(IHomeSchoolingRepository context, UserManager<User> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            _context = context;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<IActionResult> Index(string name = null)
        {
            IdentityResult addUser = await userManager.CreateAsync(new User() { UserName = "bohdan.romaniuk", Email = "bohdan2307@gmail.com" }, "123456");
            IdentityResult result1 = await roleManager.CreateAsync(new IdentityRole("Admin"));
            IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Teacher"));
            IdentityResult result3 = await roleManager.CreateAsync(new IdentityRole("Student"));

            User user = await userManager.FindByIdAsync("b93abd87-23ea-49ca-b23e-232ef44ea03b");
            if (user != null)
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, "Student");
            }

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

        [Authorize(Roles = "Student")]
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
