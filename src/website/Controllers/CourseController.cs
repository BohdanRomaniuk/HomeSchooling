using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using Microsoft.AspNetCore.Http;
using database.Models;

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
            Lesson currentLesson = db.Lessons.Include(l => l.Posts).ThenInclude(l=>l.PostAtachments).Where(l => l.Id == id).SingleOrDefault();
            return View(new LessonViewModel(currentLesson.Name, currentLesson.Posts));
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            return View("AddCourse");
        }

        [HttpPost]
        public IActionResult AddCourse(string courseName, string courseDescription)
        {
            //Needed session handling
            User teacher = db.Users.Where(u => u.Id == 2).SingleOrDefault();
            db.Courses.Add(new Course(courseName, courseDescription, teacher));
            db.SaveChanges();
            return View("AddCourse", String.Format("Курс \"{0}\" успішно створено!", courseName));
        }

        [HttpGet]
        public IActionResult AddLesson(int id)
        {
            ViewData["CourseId"] = id;
            return View("AddLesson");
        }

        [HttpPost]
        public IActionResult AddLesson(int courseId, string lessonName, string lessonDatetime, string lessonDescription, string homeworkDescription, string isControllWork)
        {
            Course currentCourse = db.Courses.Include(c=>c.CourseLessons).Where(c => c.Id == courseId).SingleOrDefault();
            //Needed session handling
            User postedBy = db.Users.Where(u => u.Id == 2).SingleOrDefault();
            Lesson newLesson = new Lesson(lessonName, Convert.ToDateTime(lessonDatetime), Convert.ToBoolean(isControllWork));
            newLesson.Posts.Add(new Post(lessonDescription, "lesson-desc", postedBy, DateTime.Now));
            newLesson.Posts.Add(new Post(homeworkDescription, "homework-desc", postedBy, DateTime.Now));
            currentCourse.CourseLessons.Add(newLesson);
            db.SaveChanges();
            return View("AddLesson", String.Format("Урок \"{0}\" успішно додано!",lessonName));
        }

        public IActionResult RequestCourse(int id)
        {
            if(HttpContext.Session.GetInt32("role") !=null)
            {
                //string role = HttpContext.Session.GetString("role");
                //int studId = Convert.ToInt32(HttpContext.Session.GetString("id"),0);
                string role = "student";
                if(role=="student")
                {
                    //db.CoursesListeners.Add(new database.Models.CoursesListener(3, 5, id));
                    return View("RequestCourse", "Вас успішно записано на курс");
                }
                else
                {
                    return View("RequestCourse", "Вибачте ви не студент!");
                }
            }
            return View("Виникла помилка при записуванні на курс");
        }
    }
}