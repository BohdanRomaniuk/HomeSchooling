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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace website.Controllers
{
    public class CourseController : Controller
    {
        private readonly IHomeSchoolingRepository db;
        private readonly IFileProvider fileProvider;
        private UserManager<User> userManager;

        public CourseController(IHomeSchoolingRepository _db, IFileProvider _fileProvider, UserManager<User> _userManager)
        {
            db = _db;
            fileProvider = _fileProvider;
            userManager = _userManager;
        }
        
        
        public IActionResult ViewCourse(int id)
        {
            Course currentCourse = db.Courses.Include(c => c.Teacher).Include(c => c.CourseLessons).Where(o => o.Id == id).SingleOrDefault();
            ViewData["IsCourseOwner"] = currentCourse.Teacher.UserName == HttpContext?.User?.Identity?.Name;
            return View(currentCourse);
        }

        [Authorize]
        public IActionResult ViewLesson(int id)
        {
            ViewData["IsCourseListener"] = false;
            Lesson currentLesson = db.Lessons
                .Include(lesson => lesson.Posts).ThenInclude(post => post.PostAtachments).ThenInclude(attach => attach.UploadedBy)
                .Include(lesson => lesson.Posts).ThenInclude(post => post.PostedBy).Where(lesson => lesson.Id == id).SingleOrDefault();
            string userName = User.Identity.Name;
            int currentCourseId = db.Courses.Where(c => c.CourseLessons.Where(l => l.Id == id).Count() != 0).SingleOrDefault().Id;
            if (HttpContext.User.IsInRole("Teacher"))
            {
                ViewData["IsCourseListener"] = db.Courses.Include(c => c.Teacher).Where(c => c.Id == currentCourseId).FirstOrDefault().Teacher.UserName == userName;
            }
            else if(HttpContext.User.IsInRole("Student"))
            {
                ViewData["IsCourseListener"] = db.CoursesListeners.Where(s => s.Student.UserName == userName && s.Accepted && s.RequestedCourse.Id == currentCourseId).Count() != 0;
            }
            else if(HttpContext.User.IsInRole("Admin"))
            {
                ViewData["IsCourseListener"] = true;
            }
            return View(new LessonViewModel(currentLesson));
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddCourse(string courseName, string courseDescription)
        {
            User teacher = await userManager.GetUserAsync(User);
            //Course restictions!!!!
            //db.AddCourse(new Course(courseName, courseDescription, teacher));
            return View("AddCourse", String.Format("Курс \"{0}\" успішно створено!", courseName));
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddLesson(int id)
        {
            string teacherId = (await userManager.GetUserAsync(User)).Id;
            Course currentCourse = db.Courses.Include(c => c.Teacher).Where(c => c.Id == id).SingleOrDefault();
            if (teacherId == currentCourse.Teacher.Id)
            {
                ViewData["CourseId"] = id;
                return View("AddLesson");
            }
            else
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddLesson(int courseId, string lessonName, string lessonStartDatetime, string lessonEndDatetime, string lessonDescription, string homeworkDescription, string homeworkEndDatetime, string isControllWork, List<IFormFile> files1, List<IFormFile> files2)
        {
            string teacherId = (await userManager.GetUserAsync(User)).Id;
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
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.GetFilename());
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
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.GetFilename());
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

            db.AddLesson(courseId, newLesson);
            return View("AddLesson", String.Format("Урок \"{0}\" успішно додано!", lessonName));
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AddHomeWork(int lessonId, string homeWorkDescription, List<IFormFile> files)
        {
            string studentId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
            User currentStudent = db.Users.Where(u => u.Id == studentId).SingleOrDefault();
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

            db.AddHomeWork(lessonId, newPost);
            return RedirectToRoute(new { controller = "Course", action = "ViewLesson", id = lessonId });
        }

        [Authorize(Roles = "Student")]
        public IActionResult RequestCourse(int id)
        {
            var acc = db.CoursesListeners.Where(c => c.Student.UserName == HttpContext.User.Identity.Name).Where(c => c.RequestedCourse.Id == id);
            if (acc.Count() == 0)
            {
                CoursesListener cl = new CoursesListener();
                cl.Accepted = false;
                cl.RequestedCourse = db.Courses.Where(c => c.Id == id).SingleOrDefault();
                cl.Student = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
                db.AddCourseListener(cl);
                return View("RequestCourse", "Ви подали заявку на курс, викладач розгляне її найближчим часом.");
            }
            else if (acc.SingleOrDefault().Accepted == true)
            {
                return RedirectToRoute(new { controller = "Course", action = "ViewCourse", id = id });
            }
            else
            {
                return View("RequestCourse", "Ви вже подали заявку на курс, але викладач ще її не розглянув");
            }
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult ViewRequests()
        {
            var model = from courses in db.Courses.Include(o => o.Teacher)
                        where courses.Teacher.UserName == HttpContext.User.Identity.Name
                        join listeners in db.CoursesListeners.Include(o => o.Student) on courses.Id equals listeners.RequestedCourse.Id
                        where listeners.Accepted == false
                        select new
                        {
                            CourseId = courses.Id,
                            CourseName = courses.Name,
                            StudentName = listeners.Student.Name,
                            StudentUserName = listeners.Student.UserName,
                        };
            List<CourseRequestModel> vm = new List<CourseRequestModel>();
            foreach (var m in model)
            {
                vm.Add(new CourseRequestModel(m.CourseId, m.CourseName, m.StudentName, m.StudentUserName));
            }
            return View(vm);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult AcceptCourse(string student_name, int course_id)
        {
            db.AcceptCourse(student_name, course_id);
            return RedirectToRoute(new { controller = "Course", action = "ViewRequests" });
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult RefuseCourse(string student_name, int course_id)
        {
            db.RefuseCourse(student_name, course_id);    
            return RedirectToRoute(new { controller = "Course", action = "ViewRequests" });
        }
    }
}