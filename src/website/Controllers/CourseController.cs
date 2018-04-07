using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using Microsoft.AspNetCore.Http;

namespace website.Controllers
{
    public class CourseController : Controller
    {
        private readonly HomeSchoolingContext db;

        public CourseController(HomeSchoolingContext _db)
        {
            db = _db;
        }
        
        public IActionResult ViewCourse(int id)
        {
            return View(db.Courses.Include(c=>c.Teacher).Include(c=>c.CourseLessons).Where(o=>o.Id==id).SingleOrDefault());
        }

        public IActionResult ViewLesson(int id)
        {
            return View(db.Lessons.Where(l=>l.Id==id).SingleOrDefault());
        }

        public IActionResult RequestCourse(int id)
        {
            if(HttpContext.Session.GetInt32("role") !=null)
            {
                string role = HttpContext.Session.GetString("role");
                int studId = int.Parse(HttpContext.Session.GetString("id"));
                if(role=="student")
                {
                    bool? acc;
                    try
                    {
                        acc = db.CoursesListeners.Where(c => c.Student.Id == studId).Where(c => c.RequestedCourse.Id == id).SingleOrDefault().Accepted;
                    }
                    catch (Exception e)
                    {
                        acc = null;
                    }
                    if (acc == null)
                    {
                        database.Models.CoursesListener cl = new database.Models.CoursesListener();
                        cl.Accepted = false;
                        cl.RequestedCourse = db.Courses.Where(c => c.Id == id).SingleOrDefault();
                        cl.Student = db.Users.Where(u => u.Id == studId).SingleOrDefault();
                        db.CoursesListeners.Add(cl);
                        db.SaveChanges();
                        return View("RequestCourse", "Ви подали заявку на курс, викладач розгляне її найближчим часом.");
                    }
                    else
                    {
                        
                        if (acc == true)
                        {
                            return RedirectToRoute(new { controller = "Course", action = "ViewCourse", RouteData = id });
                        }
                        else
                        {
                            return View("RequestCourse", "Ви вже подали заявку на курс, але викладач ще її не розглянув");
                        }
                    }
                }
                else
                {
                    return View("RequestCourse", "Тільки студенти можуть подавати заявку на курс");
                }
            }
            return RedirectToRoute(new { controller = "Profile", action = "Login" });
        }
        public IActionResult ViewRequests()
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                int id = int.Parse(HttpContext.Session.GetString("id"));
                if (role == "teacher")
                {
                    var model = from courses in db.Courses.Include(o => o.Teacher)
                                where courses.Teacher.Id == id
                                join listeners in db.CoursesListeners.Include(o => o.Student) on courses.Id equals listeners.RequestedCourse.Id
                                where listeners.Accepted == false
                                select new
                                {
                                    CourseId = courses.Id,
                                    CourseName = courses.Name,
                                    StudentName = listeners.Student.Name,
                                    StudentUserName = listeners.Student.UserName,
                                    StudentId = listeners.Student.Id
                                };
                    List<CourseRequestModel> vm = new List<CourseRequestModel>();
                    foreach (var m in model)
                    {
                        vm.Add(new CourseRequestModel(m.CourseId, m.StudentId, m.CourseName, m.StudentName, m.StudentUserName));
                    }
                    return View(vm);
                }
            }
            return RedirectToRoute(new { controller = "Profile", action = "Login" });
        }
        public IActionResult AcceptCourse(int student_id, int course_id)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "teacher")
                {
                    try
                    {
                        database.Models.CoursesListener listener = (from listeners in db.CoursesListeners
                                        where listeners.Student.Id == student_id && listeners.RequestedCourse.Id == course_id
                                        select listeners).SingleOrDefault();
                        listener.Accepted = true;
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            return RedirectToRoute(new { controller = "Course", action = "ViewRequests" });
        }
        public IActionResult RefuseCourse(int student_id, int course_id)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "teacher")
                {
                    try
                    {
                        database.Models.CoursesListener listener = (from listeners in db.CoursesListeners
                                                                    where listeners.Student.Id == student_id && listeners.RequestedCourse.Id == course_id
                                                                    select listeners).SingleOrDefault();
                        db.CoursesListeners.Remove(listener);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            return RedirectToRoute(new { controller = "Course", action = "ViewRequests" });
        }
    }
}