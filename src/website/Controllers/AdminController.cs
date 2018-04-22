using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace website.Controllers
{
    public class AdminController : Controller
    {
        private IHomeSchoolingRepository db;
        private UserManager<User> userManager;
        public AdminController(IHomeSchoolingRepository _db, UserManager<User> _userManager)
        {
            userManager = _userManager;
            db = _db;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetTeacher(string name)
        {
            //db.SetTeacher(name);
            User teacher = db.Users.Where(u => u.UserName == name).SingleOrDefault();
            await userManager.RemoveFromRoleAsync(teacher, "Student");
            await userManager.AddToRoleAsync(teacher, "Teacher");
            return RedirectToRoute(new { controller = "Profile", action = "View", name = name });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCourse(int id)
        {
            db.DeleteCourse(id);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}