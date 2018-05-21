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
using System.Net.Http.Headers;

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
        [Authorize(Roles = "Teacher")]
        public IActionResult ViewListeners(int id)
        {
            Course currentCourse = db.Courses.Include(c => c.Teacher).Include(c => c.CourseLessons).Where(o => o.Id == id).SingleOrDefault();
            var studs = from courselisteners in db.CoursesListeners
                        where courselisteners.RequestedCourse.Id == id
                        where courselisteners.Accepted == true
                        select courselisteners.Student;
            if (studs.Count() != 0)
            {
                int minYear = studs.First().BirthYear;
                int maxYear = studs.First().BirthYear;
                string minname = studs.First().Name;
                string maxname = studs.First().Name;
                foreach (User i in studs)
                {
                    if (i.BirthYear < minYear)
                    {
                        minYear = i.BirthYear;
                        minname = i.Name;
                    }
                    if (i.BirthYear > maxYear)
                    {
                        maxYear = i.BirthYear;
                        maxname = i.Name;
                    }
                }
                ViewData["MinYear"] = minYear;
                ViewData["Youngest"] = minname;
                ViewData["MaxYear"] = maxYear;
                ViewData["Oldest"] = maxname;
            }
            return View("Listeners");
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
                .Include(lesson => lesson.Posts).ThenInclude(post => post.PostedBy)
                .Include(lesson => lesson.Posts).ThenInclude(post => post.PostMark).ThenInclude(mark => mark.Teacher)
                .Include(lesson => lesson.Posts).ThenInclude(post => post.PostMark).ThenInclude(mark => mark.Student)
                .Where(lesson => lesson.Id == id).SingleOrDefault();
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
            var marks = from mark in currentLesson.Posts where mark.PostMark != null select mark.PostMark;
            return View(new LessonViewModel(currentLesson, marks.ToList()));
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddCourse(string courseName, string courseDescription, DateTime courseStartDatetime, DateTime courseEndDatetime)
        {
            User teacher = await userManager.FindByNameAsync(User.Identity.Name);
            db.AddCourse(new Course(courseName, courseDescription, teacher, courseStartDatetime, courseEndDatetime));
            return View("AddCourse", String.Format("Курс \"{0}\" успішно створено!", courseName));
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddLesson(int id)
        {
            string teacherId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
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
            string teacherId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
            User postedBy = db.Users.Where(u => u.Id == teacherId).SingleOrDefault();
            Lesson newLesson = new Lesson(lessonName, Convert.ToDateTime(lessonStartDatetime), Convert.ToDateTime(lessonEndDatetime), Convert.ToDateTime(homeworkEndDatetime), Convert.ToBoolean(isControllWork));
            Post lesson_post = new Post(lessonDescription, "lesson-desc", postedBy, DateTime.Now);
            Post homework_post = new Post(homeworkDescription, "homework-desc", postedBy, DateTime.Now);
            bool flag = false;
            int fileLenght = 1000000;
            int filesQuantity = 20;
            //Files1 upload begin
            if ((files1!=null && files1.Count > filesQuantity) || (files2!=null && files2.Count > filesQuantity))
                return View("AddLesson", String.Format("Урок \"{0}\" не додано ,кількість файлів перевищено!", lessonName));

            if (files1 != null && files1.Count != 0)
            {
                lesson_post.PostAtachments = new List<Attachment>();
                foreach (var file in files1)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.GetFilename());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        if (file.Length < fileLenght)
                        {
                            await file.CopyToAsync(stream);
                        }
                        else
                        {
                            flag = true;
                        }
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
                        if (file.Length < fileLenght)
                        {
                            await file.CopyToAsync(stream);
                        }
                        else
                        {
                            flag = true;
                        }
                    }

                    homework_post.PostAtachments.Add(new Attachment(file.GetFilename(), postedBy, DateTime.Now));
                }

            }
            if (flag == true)
            {
                return View("AddLesson", String.Format("Урок \"{0}\" не додано ,розмір перевищено!", lessonName));
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
            bool flag = false;
            int fileLenght = 1000000;
            int filesQuantity = 20;
            string studentId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
            User currentStudent = db.Users.Where(u => u.Id == studentId).SingleOrDefault();
            Post newPost = new Post(homeWorkDescription, "homework", currentStudent, DateTime.Now);
            //Files upload begin
            if (files!=null && files.Count > filesQuantity)
                return Content("Забагато файлів");
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
                        if (file.Length < fileLenght)
                        {
                            await file.CopyToAsync(stream);
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                        return Content("Розмір перевищено.");
                    newPost.PostAtachments.Add(new Attachment(file.GetFilename(), currentStudent, DateTime.Now));
                }
            }
            //Files upload end

            db.AddHomeWork(lessonId, newPost);
            return RedirectToRoute(new { controller = "Course", action = "ViewLesson", id = lessonId });
        }

        [HttpGet]
        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> ViewStudents(int id)
        {
            Course currentCourse = db.Courses.Include(c=>c.Teacher).Where(c => c.Id == id).FirstOrDefault();
            User teacher = await userManager.FindByNameAsync(User.Identity.Name);
            if(teacher.Id==currentCourse.Teacher.Id)
            {
                IQueryable<User> students = from listener in db.CoursesListeners.Include(c => c.RequestedCourse).Include(c => c.Student)
                                            where listener.Accepted == true && listener.RequestedCourse.Id == id
                                            select listener.Student;
                return View(new CourseStudentsModel(students, currentCourse.Name));
            }
            else
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> ViewLocations(int id)
        {
            Course currentCourse = db.Courses.Include(c => c.Teacher).Where(c => c.Id == id).FirstOrDefault();
            User teacher = await userManager.FindByNameAsync(User.Identity.Name);
            if (teacher.Id == currentCourse.Teacher.Id)
            {
                IQueryable<User> students = from listener in db.CoursesListeners.Include(c => c.RequestedCourse).Include(c => c.Student)
                                            where listener.Accepted == true && listener.RequestedCourse.Id == id
                                            select listener.Student;
                return View(new CourseStudentsModel(students.GroupBy(g => g.Location).Select(x => x.FirstOrDefault()), currentCourse.Name));
            }
            else
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
        }

        [HttpPost]
        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> AddMark(string MarkValue, string PostId, string LessonId)
        {
            User teacher = await userManager.FindByNameAsync(User.Identity.Name);
            db.AddMark(Convert.ToInt32(PostId), Convert.ToInt32(LessonId), Convert.ToInt32(MarkValue), teacher);
            return RedirectToRoute(new { controller = "Course", action = "ViewLesson", id = Convert.ToInt32(LessonId) });
        }

        [Authorize(Roles = "Student")]
        public IActionResult RequestCourse(int id)
        {
            var acc = db.CoursesListeners.Where(c => c.Student.UserName == HttpContext.User.Identity.Name).Where(c => c.RequestedCourse.Id == id);
            if (acc.Count() == 0)
            {
                if (db.Courses.Where(c => c.Id == id).SingleOrDefault().StartDate < DateTime.Now)
                {
                    return View("RequestCourse", "Курс вже почався, ви не можете подавати на нього заявку");
                }
                else
                {
                    CoursesListener cl = new CoursesListener();
                    cl.Accepted = false;
                    cl.RequestedCourse = db.Courses.Where(c => c.Id == id).SingleOrDefault();
                    cl.Student = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
                    db.AddCourseListener(cl);
                    return View("RequestCourse", "Ви подали заявку на курс, викладач розгляне її найближчим часом.");
                }
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
        [Authorize(Roles = "Student")]
        public IActionResult DeleteFromCourse(int id)
        {
            var acc = db.CoursesListeners.Where(c => c.Student.UserName == HttpContext.User.Identity.Name).Where(c => c.RequestedCourse.Id == id);
            if (acc.Count() == 0)
            {
                return View("RequestCourse", "Ви не відвідуєте даний курс");
            }
            else if (acc.SingleOrDefault().Accepted == true)
            {
                db.RefuseCourse(HttpContext.User.Identity.Name, id);
                return RedirectToRoute(new { controller = "Home", action = "Index"});
            }
            else
            {
                return View("RequestCourse", "Ви ще не відвідуєте даний курс");
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