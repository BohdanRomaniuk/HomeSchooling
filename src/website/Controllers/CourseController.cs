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
        private readonly IHomeSchoolingRepository db;
        private readonly IFileProvider fileProvider;

        public CourseController(IHomeSchoolingRepository _db, IFileProvider _fileProvider)
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
                    isCourseOwner = (currentCourse.Teacher.Id == teacherId) ? true : false;
                }
            }
            ViewData["IsCourseOwner"] = isCourseOwner;
            return View(currentCourse);
        }

        public IActionResult ViewLesson(int id)
        {
            ViewData["IsCourseListener"] = false;
            Lesson currentLesson = db.Lessons
                .Include(lesson => lesson.Posts).ThenInclude(post => post.PostAtachments).ThenInclude(attach => attach.UploadedBy)
                .Include(lesson => lesson.Posts).ThenInclude(post => post.PostedBy).Where(lesson => lesson.Id == id).SingleOrDefault();
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));
                int currentCourseId = db.Courses.Where(c => c.CourseLessons.Where(l => l.Id == id).Count() != 0).SingleOrDefault().Id;
                if (role == "student")
                {
                    ViewData["IsCourseListener"] = db.CoursesListeners.Where(s => s.Student.Id == userId && s.Accepted && s.RequestedCourse.Id == currentCourseId).Count() != 0;
                }
                else if (role == "teacher")
                {
                    ViewData["IsCourseListener"] = db.Courses.Where(c=>c.Teacher.Id==userId).Count() != 0;
                }
            }
            return View(new LessonViewModel(currentLesson));
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
                    //db.Courses.Add(new Course(courseName, courseDescription, teacher));
                    //db.SaveChanges();
                    //HomeSchooling context changes
                    db.AddCourse(new Course(courseName, courseDescription, teacher));
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
        public async Task<IActionResult> AddLesson(int courseId, string lessonName, string lessonStartDatetime, string lessonEndDatetime, string lessonDescription, string homeworkDescription, string homeworkEndDatetime, string isControllWork, List<IFormFile> files1, List<IFormFile> files2)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "teacher")
                {
                    int teacherId = int.Parse(HttpContext.Session.GetString("id"));
                    //Course currentCourse = db.Courses.Include(c => c.CourseLessons).Where(c => c.Id == courseId).SingleOrDefault();
                    User postedBy = db.Users.Where(u => u.Id == teacherId).SingleOrDefault();
                    Lesson newLesson = new Lesson(lessonName, Convert.ToDateTime(lessonStartDatetime), Convert.ToDateTime(lessonEndDatetime), Convert.ToDateTime(homeworkEndDatetime), Convert.ToBoolean(isControllWork));
                    Post lesson_post = new Post(lessonDescription, "lesson-desc", postedBy, DateTime.Now);
                    Post homework_post = new Post(homeworkDescription, "homework-desc", postedBy, DateTime.Now);
                   
                    //Files1 upload begin
                    if (files1 != null && files1.Count != 0)
                    {
                        lesson_post.PostAtachments = new List<Attachment>();
                        foreach (var file in files1)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",file.GetFilename());
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            lesson_post.PostAtachments.Add(new Attachment(file.GetFilename(), postedBy, DateTime.Now));
                        }
                    }
                    //Files1 upload end
                    //Files2 upload begin
                    if (files2 != null && files2.Count != 0)
                    {
                        homework_post.PostAtachments = new List<Attachment>();
                        foreach (var file in files2)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",file.GetFilename());
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            homework_post.PostAtachments.Add(new Attachment(file.GetFilename(), postedBy, DateTime.Now));
                        }
                    }
                    //Files2 upload end
                    newLesson.Posts.Add(lesson_post);
                    newLesson.Posts.Add(homework_post);
                    //currentCourse.CourseLessons.Add(newLesson);
                    //db.SaveChanges();
                    db.AddLesson(courseId, newLesson);
                    return View("AddLesson", String.Format("Урок \"{0}\" успішно додано!", lessonName));
                }
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return RedirectToRoute(new { controller = "Profile", action = "Login" });
        }

        [HttpPost]
        public async Task<IActionResult> AddHomeWork(int lessonId, string homeWorkDescription, List<IFormFile> files)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                if (role == "student")
                {
                    int studentId = int.Parse(HttpContext.Session.GetString("id"));
                    User currentStudent = db.Users.Where(u => u.Id == studentId).SingleOrDefault();
                    //Lesson currentLesson = db.Lessons.Where(l => l.Id == lessonId).SingleOrDefault();
                    Post newPost = new Post(homeWorkDescription, "homework", currentStudent, DateTime.Now);

                    //Files upload begin
                    if (files != null && files.Count != 0)
                    {
                        newPost.PostAtachments = new List<Attachment>();
                        foreach (var file in files)
                        {
                            var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot",
                                    file.GetFilename());

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            newPost.PostAtachments.Add(new Attachment(file.GetFilename(), currentStudent, DateTime.Now));
                        }
                    }
                    //Files upload end
                    //currentLesson.Posts.Add(new_post);
                    //db.SaveChanges();
                    db.AddHomeWork(lessonId, newPost);
                    return RedirectToRoute(new { controller = "Course", action = "ViewLesson", id = lessonId });
                }
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return RedirectToRoute(new { controller = "Profile", action = "Login" });
        }

        public IActionResult RequestCourse(int id)
        {
            if (HttpContext.Session.GetInt32("role") != null)
            {
                string role = HttpContext.Session.GetString("role");
                int studId = int.Parse(HttpContext.Session.GetString("id"));
                if (role == "student")
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
                        CoursesListener cl = new CoursesListener();
                        cl.Accepted = false;
                        cl.RequestedCourse = db.Courses.Where(c => c.Id == id).SingleOrDefault();
                        cl.Student = db.Users.Where(u => u.Id == studId).SingleOrDefault();
                        //db.CoursesListeners.Add(cl);
                        //db.SaveChanges();
                        db.AddCourseListener(cl);
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
                        //database.Models.CoursesListener listener = (from listeners in db.CoursesListeners
                        //                                            where listeners.Student.Id == student_id && listeners.RequestedCourse.Id == course_id
                        //                                            select listeners).SingleOrDefault();
                        //listener.Accepted = true;
                        //db.SaveChanges();
                        db.AcceptCourse(student_id, course_id);
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
                        //database.Models.CoursesListener listener = (from listeners in db.CoursesListeners
                        //                                            where listeners.Student.Id == student_id && listeners.RequestedCourse.Id == course_id
                        //                                            select listeners).SingleOrDefault();
                        //db.CoursesListeners.Remove(listener);
                        //db.SaveChanges();
                        db.RefuseCourse(student_id, course_id);
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