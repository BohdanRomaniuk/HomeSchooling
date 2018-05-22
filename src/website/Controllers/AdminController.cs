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

        [HttpGet]
        public IActionResult ApprovingList()
        {
            IQueryable<User> notApprovedUsers = db.Users.Where(u => u.Approved == false);
            return View(notApprovedUsers);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCourse(int id)
        {
            db.DeleteCourse(id);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ApproveUser(string username)
        {
            db.ApproveUserToSystem(username);
            return RedirectToRoute(new { controller = "Admin", action = "ApprovingList" });
        }
        [Authorize(Roles = "Admin")]
        public IActionResult RejectUser(string username)
        {
            db.RejectUserToSystem(username);
            return RedirectToRoute(new { controller = "Admin", action = "ApprovingList" });
        }
    }
}