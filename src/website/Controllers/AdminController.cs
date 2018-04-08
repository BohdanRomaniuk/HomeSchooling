using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using database.Models;
namespace website.Controllers
{
    public class AdminController : Controller
    {
        private HomeSchoolingContext db;
        public AdminController(HomeSchoolingContext context)
        {
            db = context;
        }
        public IActionResult SetTeacher(int id)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "admin")
                {
                    User teacher = db.Users.Where(u => u.Id == id).SingleOrDefault();
                    teacher.UserRole = "teacher";
                    db.SaveChanges();
                }
            }
            return RedirectToRoute(new { controller = "Profile", action = "View", id = id });
        }
    }
}