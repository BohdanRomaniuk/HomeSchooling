﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace website.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        public ProfileController(UserManager<User> _userManager, SignInManager<User> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Info = "Введіть необхідні дані для реєстрації";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = new User {
                    UserName = details.UserName,
                    Email = details.Email
        //          Name = details.Name
                };
                IdentityResult result = await userManager.CreateAsync(user, details.Password);
                IdentityResult result2 = await userManager.AddToRoleAsync(user, "Student");
                Microsoft.AspNetCore.Identity.SignInResult result1 = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                if (result.Succeeded && result1.Succeeded && result2.Succeeded)
                {
                    return Redirect(returnUrl ?? "/");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(details);
        }
       
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Info = "Введіть свій логін та пароль, щоб увійти у систему";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByNameAsync(details.UserName);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.UserName), "Неправильне і'мя користувача чи пароль!");
            }
            return View(details);
        }

        //[HttpPost]
        //public IActionResult Login(User user)
        //{
        //    if (user.UserName == null)
        //    {
        //        ViewData["IncorrectLogin"] = true;
        //        ViewBag.Info = "Потрібно заповнити поле з іменем користувача";
        //        return View();
        //    }
        //    else if (user.Password == null)
        //    {
        //        ViewData["IncorrectPassword"] = true;
        //        ViewBag.Info = "Потрібно заповнити поле з паролем";
        //        return View();
        //    }

        //    IQueryable<User> users = db.Users;
        //    bool exists = false;
        //    bool correctpassword = false;

        //    foreach (User u in users)
        //    {
        //        if (u.UserName == user.UserName)
        //        {
        //            exists = true;
        //            if (u.Password == user.Password)
        //            {
        //                correctpassword = true;
        //                // add session
        //                HttpContext.Session.SetString("username", u.UserName);
        //                HttpContext.Session.SetString("role", u.UserRole);
        //                HttpContext.Session.SetString("name", u.Name);
        //                HttpContext.Session.SetString("id", u.Id.ToString());
        //            }
        //            break;
        //        }
        //    }
        //    if (!exists)
        //    {
        //        ViewData["IncorrectLogin"] = true;
        //        ViewBag.Info = "Користувача з таким іменем нема в системі. Можливо, ви ще не зареєстровані";
        //        return View();
        //    }
        //    else if (!correctpassword)
        //    {
        //        ViewData["IncorrectPassword"] = true;
        //        ViewBag.Info = "Ви ввели неправильний пароль";
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToRoute(new { controller = "Home", action = "Index" });
        //    }
        //}

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        //public IActionResult View(int id)
        //{
        //    try
        //    {
        //        ViewData["name"] = db.Users.Where(u => u.Id == id).SingleOrDefault().Name;
        //        ViewData["username"] = db.Users.Where(u => u.Id == id).SingleOrDefault().UserName;
        //        ViewData["id"] = id;
        //        string role = db.Users.Where(u => u.Id == id).SingleOrDefault().UserRole;
        //        ViewData["role"] = role;
        //        if (role == "teacher")
        //        {
        //            return View(db.Courses.Include(o => o.Teacher).Where(c => c.Teacher.Id == id));
        //        }
        //        else
        //        {
        //            var courses = from course in db.Courses.Include(c => c.Teacher)
        //                          join listener in db.CoursesListeners on course.Id equals listener.RequestedCourse.Id
        //                          where listener.Student.Id == id
        //                          where listener.Accepted == true
        //                          select course;
        //            return View(courses);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
        //}
    }
}