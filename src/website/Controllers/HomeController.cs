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

        public IActionResult Index(string name = null)
        {
            IQueryable<Course> allCourses;
            if (name == null)
            {
                allCourses = _context.Courses.Include(c => c.Teacher).Include(c => c.CourseLessons);
            }
            else
            {
                allCourses = _context.Courses.Include(c => c.Teacher).Include(c => c.CourseLessons).Where(s => s.Name.Contains(name));
            }
            List<HomeViewModel> vm = new List<HomeViewModel>();
            foreach (var course in allCourses)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    bool? show;
                    var acc = _context.CoursesListeners.Where(c => c.Student.UserName == HttpContext.User.Identity.Name).Where(c => c.RequestedCourse.Id == course.Id);
                    if (acc.Count() == 0)
                    {
                        if (_context.Courses.Where(c => c.Id == course.Id).SingleOrDefault().StartDate < DateTime.Now)
                        {
                            //show nothing
                            show = null;
                        }
                        else
                        {
                            //show request
                            show = true;
                        }
                    }
                    else if (acc.SingleOrDefault().Accepted == true)
                    {
                        //show delete
                        show = false;
                    }
                    else
                    {
                        //show nothing
                        show = null;
                    }
                    vm.Add(new HomeViewModel(course, show));
                }
                else
                {
                    vm.Add(new HomeViewModel(course, null));
                }
            }
            return View(vm);
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

        public async Task<IActionResult> CreateDB()
        {
            //Roles
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Teacher"));
            await roleManager.CreateAsync(new IdentityRole("Student"));

            //Users
            await userManager.CreateAsync(new User() { UserName = "bohdan.romaniuk", Name="Романюк Богдан", Email = "bohdan2307@gmail.com" }, "123456");
            await userManager.CreateAsync(new User() { UserName = "roman.parobiy", Name="Паробій Роман", Email = "roman.parobiy@gmail.com" }, "123456");
            await userManager.CreateAsync(new User() { UserName = "modest.radomskyy", Name="Модест Радомський", Email = "modest.radomskyy@gmail.com" }, "123456");
            await userManager.CreateAsync(new User() { UserName = "anatoliy.muzychuk", Name="Музичук А.О.", Email = "anatoliy.muzychuk@gmail.com" }, "123456");
            await userManager.CreateAsync(new User() { UserName = "sviatoslav.tarasyuk", Name = "Тарасюк С.І.", Email = "sviatoslav.tarasyuk@gmail.com" }, "123456");
            await userManager.CreateAsync(new User() { UserName = "svyatoslav.litynskyy", Name = "Літинський С.В.", Email = "svyatoslav.litynskyy@gmail.com" }, "123456");
            await userManager.CreateAsync(new User() { UserName = "admin", Name = "Адміністратор", Email = "admin@admin.com" }, "123456");

            User bohdan = await userManager.FindByNameAsync("bohdan.romaniuk");
            User roman = await userManager.FindByNameAsync("roman.parobiy");
            User modest = await userManager.FindByNameAsync("modest.radomskyy");
            User muzychuk = await userManager.FindByNameAsync("anatoliy.muzychuk");
            User tarasyuk = await userManager.FindByNameAsync("sviatoslav.tarasyuk");
            User litynskyy = await userManager.FindByNameAsync("svyatoslav.litynskyy");
            User admin = await userManager.FindByNameAsync("admin");

            await userManager.AddToRoleAsync(bohdan, "Student");
            await userManager.AddToRoleAsync(roman, "Student");
            await userManager.AddToRoleAsync(modest, "Student");
            await userManager.AddToRoleAsync(muzychuk, "Teacher");
            await userManager.AddToRoleAsync(tarasyuk, "Teacher");
            await userManager.AddToRoleAsync(litynskyy, "Teacher");
            await userManager.AddToRoleAsync(admin, "Admin");

            database.Database.Main(new string[] { "Start" });
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}
