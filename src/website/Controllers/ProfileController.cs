using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using database.Models;
using Microsoft.AspNetCore.Http;

namespace website.Controllers
{
    public class ProfileController : Controller
    {
        private HomeSchoolingContext db;
        public ProfileController(HomeSchoolingContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Info = "Введіть необхідні дані для реєстрації";
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            bool wrong = false;
            if (user.UserName == null)
            {
                ViewBag.IncorrectLogin = true;
                ViewBag.Info = "Потрібно заповнити поле з іменем користувача \n";
                wrong = true;
            }
            if (user.Password == null)
            {
                ViewBag.IncorrectPassword = true;
                ViewBag.Info1 = "Потрібно заповнити поле з паролем \n";
                wrong = true;
            }
            if (user.Name == null)
            {
                ViewBag.IncorrectName = true;
                ViewBag.Info2 = "Потрібно заповнити поле з іменем та прізвищем";
                wrong = true;
            }
            if (wrong)
            {
                return View();
            }
            else
            {
                IQueryable<User> users = db.Users;
                foreach (User u in users)
                {
                    if (u.UserName == user.UserName)
                    {
                        ViewBag.IncorrectName = true;
                        ViewBag.Info = "Користувач з таким логіном уже зареєстрований в системі. Будь ласка, виберіть інший";
                        return View();
                    }
                }
                User toAdd = new User();
                toAdd.Name = user.Name;
                toAdd.Password = user.Password;
                toAdd.UserName = user.UserName;
                toAdd.UserRole = "student";
                db.Users.Add(toAdd);
                db.SaveChanges();
                IQueryable<User> inDb = from u in db.Users
                                        where u.UserName == user.UserName
                                        select u;
                HttpContext.Session.SetString("username", toAdd.UserName);
                HttpContext.Session.SetString("role", toAdd.UserRole);
                HttpContext.Session.SetString("name", toAdd.Name);
                HttpContext.Session.SetString("id", inDb.Last().Id.ToString());
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Info = "Введіть свій логін та пароль, щоб увійти у систему";
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (user.UserName == null)
            {
                ViewBag.IncorrectLogin = true;
                ViewBag.Info = "Потрібно заповнити поле з іменем користувача";
                return View();
            }
            else if (user.Password == null)
            {
                ViewBag.IncorrectPassword = true;
                ViewBag.Info = "Потрібно заповнити поле з паролем";
                return View();
            }

            IQueryable<User> users = db.Users;
            bool exists = false;
            bool correctpassword = false;

            foreach (User u in users)
            {
                if (u.UserName == user.UserName)
                {
                    exists = true;
                    if (u.Password == user.Password)
                    {
                        correctpassword = true;
                        // add session
                        HttpContext.Session.SetString("username", u.UserName);
                        HttpContext.Session.SetString("role", u.UserRole);
                        HttpContext.Session.SetString("name", u.Name);
                        HttpContext.Session.SetString("id", u.Id.ToString());
                    }
                    break;
                }
            }
            if (!exists)
            {
                ViewBag.Info = "Користувача з таким іменем нема в системі. Можливо, ви ще не зареєстровані";
                return View();
            }
            else if (!correctpassword)
            {
                ViewBag.IncorrectPassword = true;
                ViewBag.Info = "Ви ввели неправильний пароль";
                return View();
            }
            else
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        public IActionResult View(int id)
        {
           try
            {
                ViewData["name"] = db.Users.Where(u => u.Id == id).SingleOrDefault().Name;
                ViewData["username"] = db.Users.Where(u => u.Id == id).SingleOrDefault().UserName;
                ViewData["id"] = id;
                string role = db.Users.Where(u => u.Id == id).SingleOrDefault().UserRole;
                ViewData["role"] = role;
                if (role == "teacher")
                {
                    return View(db.Courses.Include(o => o.Teacher).Where(c => c.Teacher.Id == id));
                }
                else
                {
                    var courses = from course in db.Courses.Include(c => c.Teacher)
                                  join listener in db.CoursesListeners on course.Id equals listener.RequestedCourse.Id
                                  where listener.Student.Id == id
                                  where listener.Accepted == true
                                  select course;
                    return View(courses);
                }
             }
             catch (Exception e)
             {
                 return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
             }
        }
    }
}