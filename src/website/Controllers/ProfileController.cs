using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                HttpContext.Session.SetInt32("id", inDb.Last().Id);
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
                        HttpContext.Session.SetInt32("id", u.Id);
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
    }
}