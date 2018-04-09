using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using Microsoft.AspNetCore.Http;
using database.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace website.Controllers
{
    public class CourseController : Controller
    {
        private readonly HomeSchoolingContext db;
        private readonly IFileProvider fileProvider;

        public CourseController(HomeSchoolingContext _db, IFileProvider _fileProvider)
        {
            db = _db;
            fileProvider = _fileProvider;
        }
        
        public IActionResult ViewCourse(int id)
        {
            Course currentCourse = db.Courses.Include(c => c.Teacher).Include(c => c.CourseLessons).Where(o => o.Id == id).SingleOrDefault();
            bool isCourseOwner = false;
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "teacher")
                {
                    int teacherId = int.Parse(HttpContext.Session.GetString("id"));
                    isCourseOwner = (currentCourse.Teacher.Id==teacherId) ? true : false;
                }
            }
            ViewData["IsCourseOwner"] = isCourseOwner;
            return View(currentCourse);
        }

        public IActionResult ViewLesson(int id)
        {
            Lesson currentLesson = db.Lessons
                .Include(lesson => lesson.Posts).ThenInclude(post => post.PostAtachments).ThenInclude(attach=>attach.UploadedBy)
                .Include(lesson => lesson.Posts).ThenInclude(post=>post.PostedBy).Where(lesson => lesson.Id == id).SingleOrDefault();
            return View(new LessonViewModel(currentLesson.Id, currentLesson.Name, currentLesson.Posts));
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "teacher")
                {
                    return View("AddCourse");
                }
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return RedirectToRoute(new { controller = "Profile", action = "Login" });
            
        }

        [HttpPost]
        public IActionResult AddCourse(string courseName, string courseDescription)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if(role=="teacher")
                {
                    int teacherId = int.Parse(HttpContext.Session.GetString("id"));
                    User teacher = db.Users.Where(u => u.Id == teacherId).SingleOrDefault();
                    db.Courses.Add(new Course(courseName, courseDescription, teacher));
                    db.SaveChanges();
                    return View("AddCourse", String.Format("Курс \"{0}\" успішно створено!", courseName));
                }
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return RedirectToRoute(new { controller = "Profile", action = "Login" });
        }

        [HttpGet]
        public IActionResult AddLesson(int id)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "teacher")
                {
                    int teacherId = int.Parse(HttpContext.Session.GetString("id"));
                    Course currentCourse = db.Courses.Include(c => c.Teacher).Where(c => c.Id == id).SingleOrDefault();
                    if(teacherId==currentCourse.Teacher.Id)
                    {
                        ViewData["CourseId"] = id;
                        return View("AddLesson");
                    }
                    else
                    {
                        return RedirectToRoute(new { controller = "Home", action = "Index" });
                    }
                }
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return RedirectToRoute(new { controller = "Profile", action = "Login" });
        }

        [HttpPost]
        public IActionResult AddLesson(int courseId, string lessonName, string lessonDatetime, string lessonDescription, string homeworkDescription, string isControllWork)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "teacher")
                {
                    int teacherId = int.Parse(HttpContext.Session.GetString("id"));
                    Course currentCourse = db.Courses.Include(c => c.CourseLessons).Where(c => c.Id == courseId).SingleOrDefault();
                    User postedBy = db.Users.Where(u => u.Id == teacherId).SingleOrDefault();
                    Lesson newLesson = new Lesson(lessonName, Convert.ToDateTime(lessonDatetime), Convert.ToBoolean(isControllWork));
                    newLesson.Posts.Add(new Post(lessonDescription, "lesson-desc", postedBy, DateTime.Now));
                    newLesson.Posts.Add(new Post(homeworkDescription, "homework-desc", postedBy, DateTime.Now));
                    currentCourse.CourseLessons.Add(newLesson);
                    db.SaveChanges();
                    return View("AddLesson", String.Format("Урок \"{0}\" успішно додано! <a href=\"Home/Index\">Повернутися до курсу</a>", lessonName));
                }
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return RedirectToRoute(new { controller = "Profile", action = "Login" });
        }

        [HttpPost]
        public IActionResult AddHomeWork(int lessonId, string homeWorkDescription, List<IFormFile> files)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "student")
                {
                    int studentId = int.Parse(HttpContext.Session.GetString("id"));
                    User currentStudent = db.Users.Where(u => u.Id == studentId).SingleOrDefault();
                    Lesson currentLesson = db.Lessons.Where(l => l.Id == lessonId).SingleOrDefault();
                    Post new_post = new Post(homeWorkDescription, "homework", currentStudent, DateTime.Now);
                    new_post.PostAtachments = new List<Attachment>();
                    //Files upload begin
                    if (files == null || files.Count == 0)
                        return Content("files not selected");

                    foreach (var file in files)
                    {
                        var path = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot",
                                file.GetFilename());

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyToAsync(stream);
                        }
                        new_post.PostAtachments.Add(new Attachment(file.GetFilename(),currentStudent,DateTime.Now));
                    }
                    //Files upload end
                    currentLesson.Posts.Add(new_post);
                    db.SaveChanges();
                    return RedirectToRoute(new { controller = "Course", action = "ViewLesson", id = lessonId });
                }
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return RedirectToRoute(new { controller = "Profile", action = "Login" });
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