using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using website.Models;
using database.Models;
using website.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Claims;

namespace website.tests
{
    public class CourseControllerTests
    {
        [Fact]
        public void ViewCourseStudentTesting()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", PasswordHash = "123456" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("12.12.2018 19:00"), Convert.ToDateTime("12.05.2019 18:00")) { Id = 1 };
            Course ppc = new Course("Програмування мовою с++", "Опис курсу", teacher, Convert.ToDateTime("12.12.2018 19:00"), Convert.ToDateTime("12.05.2019 18:00")) { Id = 2 };
            pps.CourseLessons = new List<Lesson>();
            ppc.CourseLessons = new List<Lesson>();
            pps.CourseLessons.Add(new Lesson("Вступ у ASP .NET Core MVC", Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")));
            pps.CourseLessons.Add(new Lesson("Use Case and Domain models", Convert.ToDateTime("12.03.2018 11:50"), Convert.ToDateTime("12.03.2018 13:10"), Convert.ToDateTime("19.03.2018 13:10")));
            pps.CourseLessons.Add(new Lesson("Побудова підсистем", Convert.ToDateTime("19.03.2018 11:50"), Convert.ToDateTime("19.03.2018 13:10"), Convert.ToDateTime("27.03.2018 13:10")));
            ppc.CourseLessons.Add(new Lesson("Вступ у мову програмування C++", Convert.ToDateTime("01.09.2017 08:30"), Convert.ToDateTime("01.09.2017 09:50"), Convert.ToDateTime("08.09.2017 09:50")));
            ppc.CourseLessons.Add(new Lesson("Типи змінних, синтаксис", Convert.ToDateTime("08.09.2017 08:30"), Convert.ToDateTime("08.09.2017 09:50"), Convert.ToDateTime("15.09.2017 09:50")));
            Course[] courses = new Course[] { pps, ppc };

            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "bohdan.romaniuk"),
                        new Claim(ClaimTypes.Role, "Student")
                    }, "Authentication"))
                }
            };

            int expectedResult = pps.CourseLessons.Count;

            //Act
            var result = (controller.ViewCourse(1) as ViewResult).Model as Course;

            //Assert
            Assert.Equal(result.CourseLessons.Count(), expectedResult);
        }

        //Check owner of the course
        [Fact]
        public void ViewCourseTeacherOwnerTesting()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", PasswordHash = "123456"};
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10")) { Id = 1 };
            Course ppc = new Course("Програмування мовою с++", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10")) { Id = 2 };
            pps.CourseLessons = new List<Lesson>();
            ppc.CourseLessons = new List<Lesson>();
            pps.CourseLessons.Add(new Lesson("Вступ у ASP .NET Core MVC", Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")));
            pps.CourseLessons.Add(new Lesson("Use Case and Domain models", Convert.ToDateTime("12.03.2018 11:50"), Convert.ToDateTime("12.03.2018 13:10"), Convert.ToDateTime("19.03.2018 13:10")));
            pps.CourseLessons.Add(new Lesson("Побудова підсистем", Convert.ToDateTime("19.03.2018 11:50"), Convert.ToDateTime("19.03.2018 13:10"), Convert.ToDateTime("27.03.2018 13:10")));
            ppc.CourseLessons.Add(new Lesson("Вступ у мову програмування C++", Convert.ToDateTime("01.09.2017 08:30"), Convert.ToDateTime("01.09.2017 09:50"), Convert.ToDateTime("08.09.2017 09:50")));
            ppc.CourseLessons.Add(new Lesson("Типи змінних, синтаксис", Convert.ToDateTime("08.09.2017 08:30"), Convert.ToDateTime("08.09.2017 09:50"), Convert.ToDateTime("15.09.2017 09:50")));
            Course[] courses = new Course[] { pps, ppc };

            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "anatoliy.muzychuk"),
                        new Claim(ClaimTypes.Role, "Teacher")
                    }, "Authentication"))
                }
            };

            //Act
            bool isOwner = (bool)(controller.ViewCourse(1) as ViewResult).ViewData["IsCourseOwner"];

            //Assert
            Assert.True(isOwner);
        }

        [Fact]
        public void ViewLessonGuestTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", PasswordHash = "123456" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10")) { Id = 1 };
            Lesson pps1 = new Lesson() { Id = 1, Name = "Вступ у ASP.NET Core MVC", LessonStartDate = Convert.ToDateTime("05.03.2018 11:50"), LessonEndDate = Convert.ToDateTime("05.03.2018 13:10"), HomeWorkEnd = Convert.ToDateTime("12.03.2018 13:10"), IsControlWork = false };
            Lesson pps2 = new Lesson() { Id = 2, Name = "Побудова підсистем", LessonStartDate = Convert.ToDateTime("05.03.2018 11:50"), LessonEndDate = Convert.ToDateTime("05.03.2018 13:10"), HomeWorkEnd = Convert.ToDateTime("12.03.2018 13:10"), IsControlWork = false };
            Lesson[] lessons = new Lesson[] { pps1, pps2 };
            Course[] courses = new Course[] { pps };
            pps.CourseLessons = lessons.ToList();


            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "bohdan.romaniuk"),
                        //new Claim(ClaimTypes.Role, "Student")
                    }, "Authentication"))
                }
            };

            //Act
            bool isCourseListener = (bool)(controller.ViewLesson(1) as ViewResult).ViewData["IsCourseListener"];

            //Assert
            Assert.False(isCourseListener);
        }

//        [Fact]
//        public void ViewLessonСourseListenerTest()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            User student = new User { Id = 2, Name = "Романюк Богдан", UserName = "bohdan.romaniuk", Password = "123456", UserRole = "student" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };

//            Lesson lesson1 = new Lesson("Вступ у ASP .NET Core MVC", Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
//            pps.CourseLessons = new List<Lesson>();
//            pps.CourseLessons.Add(lesson1);
//            CoursesListener listener1 = new CoursesListener() { Id = 1, Student = student, RequestedCourse = pps, Accepted = true };

//            Course[] courses = new Course[] { pps };
//            CoursesListener[] listeners = new CoursesListener[] { listener1 };
//            Lesson[] lessons = new Lesson[] { lesson1 };

//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
//            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
//            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "student";
//            mockSession["id"] = "2";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act
//            bool isCourseListener = (bool)(controller.ViewLesson(1) as ViewResult).ViewData["IsCourseListener"];

//            //Assert
//            Assert.True(isCourseListener);
//        }

//        [Fact]
//        public void ViewLessonСourseOwnerTest()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            User student = new User { Id = 2, Name = "Романюк Богдан", UserName = "bohdan.romaniuk", Password = "123456", UserRole = "student" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };

//            Lesson lesson1 = new Lesson("Вступ у ASP .NET Core MVC", Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
//            pps.CourseLessons = new List<Lesson>();
//            pps.CourseLessons.Add(lesson1);
//            CoursesListener listener1 = new CoursesListener() { Id = 1, Student = student, RequestedCourse = pps, Accepted = true };

//            Course[] courses = new Course[] { pps };
//            CoursesListener[] listeners = new CoursesListener[] { listener1 };
//            Lesson[] lessons = new Lesson[] { lesson1 };

//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
//            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
//            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockSession["id"] = "1";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act
//            bool isOwner = (bool)(controller.ViewLesson(1) as ViewResult).ViewData["IsCourseListener"];

//            //Assert
//            Assert.True(isOwner);
//        }

//        [Fact]
//        public void AddCourseGETGuestTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
//            Course[] courses = new Course[] { pps };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = null;
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act
//            var result = controller.AddCourse();

//            //Assert
//            Assert.True(result is RedirectToRouteResult);
//            Assert.True((result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Profile")) == true);
//            Assert.True((result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "Login")) == true);
//        }

//        [Fact]
//        public void AddCourseGETStudentTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
//            Course[] courses = new Course[] { pps };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "student";
//            mockSession["id"] = "213";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act
//            var result = controller.AddCourse();

//            //Assert
//            Assert.True(result is RedirectToRouteResult);
//            Assert.True((result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Home")) == true);
//            Assert.True((result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "Index")) == true);
//        }

//        [Fact]
//        public void AddCourseGETTeacherTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
//            Course[] courses = new Course[] { pps };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockSession["id"] = "1";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act & Assert
//            Assert.True(controller.AddCourse() is ViewResult);
//        }

//        [Fact]
//        public void AddCoursePOSTTeacher()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
//            Course[] courses = new Course[] { pps };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockSession["id"] = "1";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act & Assert
//            string courseName = "Програмування мовою C#";
//            Assert.True((controller.AddCourse(courseName, "Опис") as ViewResult).Model as string == String.Format("Курс \"{0}\" успішно створено!", courseName));
//        }

//        [Fact]
//        public void AddLessonGETGuestTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
//            Course[] courses = new Course[] { pps };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = null;
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act
//            var result = controller.AddLesson(1);

//            //Assert
//            Assert.True(result is RedirectToRouteResult);
//        }

//        [Fact]
//        public void AddLessonGETTeacherNotOwnerTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
//            Course[] courses = new Course[] { pps };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockSession["id"] = "2";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act
//            var result = controller.AddLesson(1);

//            //Assert
//            Assert.True(result is RedirectToRouteResult);
//            Assert.True((result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Home")) == true);
//            Assert.True((result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "Index")) == true);
//        }

//        [Fact]
//        public void AddLessonGETTeacherOwnerTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
//            Course[] courses = new Course[] { pps };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockSession["id"] = "1";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act & Assert
//            Assert.True(controller.AddLesson(1) is ViewResult);
//            Assert.True((int)(controller.AddLesson(1) as ViewResult).ViewData["CourseId"] == 1);
//        }

//        [Fact]
//        public void AddLessonPOSTStudentTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
//            Course[] courses = new Course[] { pps };
//            User[] users = new User[] { teacher };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
//            mock.Setup(m => m.Users).Returns(users.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "student";
//            mockSession["id"] = "213";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            string lessonName = "Вступ у мову програмування C#";
//            //Act
//            Task<IActionResult> result = controller.AddLesson(1, lessonName, "19.03.2018 11:50:00", "19.03.2018 11:50:00", "Опис уроку", "Опис домашки", "19.03.2018 11:50:00", "false", null, null);

//            //Assert
//            Assert.True(result.Result is RedirectToRouteResult);
//            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Home")) == true);
//            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "Index")) == true);
//        }

//        [Fact]
//        public void AddLessonPOSTTeacherOwnerTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
//            Course[] courses = new Course[] { pps };
//            User[] users = new User[] { teacher };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
//            mock.Setup(m => m.Users).Returns(users.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockSession["id"] = "1";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            string lessonName = "Вступ у мову програмування C#";
//            //Act
//            Task<IActionResult> result = controller.AddLesson(1, lessonName, "19.03.2018 11:50:00", "19.03.2018 11:50:00", "Опис уроку", "Опис домашки", "19.03.2018 11:50:00", "false", null, null);

//            //Assert
//            Assert.True((result.Result as ViewResult).Model as string == String.Format("Урок \"{0}\" успішно додано!", lessonName));
//        }

//        [Fact]
//        public void AddHomeWorkPOSTTeacherTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Lesson pps = new Lesson("Шаблони", DateTime.Now, DateTime.Now, DateTime.Now) { Id = 1 };
//            Lesson[] lessons = new Lesson[] { pps };
//            User[] users = new User[] { teacher };
//            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());
//            mock.Setup(m => m.Users).Returns(users.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockSession["id"] = "1";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act
//            Task<IActionResult> result = controller.AddHomeWork(1, "Виконав пункт 1", null);

//            //Assert
//            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Home")) == true);
//            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "Index")) == true);
//        }

//        [Fact]
//        public void AddHomeWorkPOSTStudentTry()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            User student = new User { Id = 2, Name = "Романюк Богдан", UserName = "bohdan.romaniuk", Password = "123456", UserRole = "student" };

//            Lesson pps = new Lesson("Шаблони", DateTime.Now, DateTime.Now, DateTime.Now) { Id = 1 };
//            Lesson[] lessons = new Lesson[] { pps };
//            User[] users = new User[] { teacher };
//            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());
//            mock.Setup(m => m.Users).Returns(users.AsQueryable());

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "student";
//            mockSession["id"] = "2";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            //Act
//            Task<IActionResult> result = controller.AddHomeWork(1, "Виконав пункт 1", null);

//            //Assert
//            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Course")) == true);
//            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "ViewLesson")) == true);
//            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("id", 1)) == true);
//        }
//        [Fact]
//        public void RequestCourseTest()
//        {
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User[] users = new User[]
//            {
//                new User { Id = 1, Name = "muzychuk", UserName = "anatoliy", Password = "a1", UserRole = "teacher" },
//                new User { Id = 2, Name = "registered", UserName = "reg", Password = "reg", UserRole = "student" }
//            };
//            Course[] courses = new Course[]
//            {
//                new Course{ Id = 1, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0] },
//                new Course{ Id = 2, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0] }
//            };
//            CoursesListener[] listeners = new CoursesListener[]
//            {
//                new CoursesListener { Id = 2, Accepted = true, RequestedCourse = courses[0], Student = users[1] }
//            };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
//            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
//            mock.Setup(m => m.Users).Returns(users.AsQueryable());
//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = users[1].UserRole;
//            mockSession["id"] = users[1].Id.ToString();
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            string expected = "Ви подали заявку на курс, викладач розгляне її найближчим часом.";
//            string actual = (controller.RequestCourse(2) as ViewResult).Model.ToString();
//            Assert.Equal(expected, actual);
//        }
//        [Fact]
//        public void ExistingRequestCourseTest()
//        {
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User[] users = new User[]
//            {
//                new User { Id = 1, Name = "muzychuk", UserName = "anatoliy", Password = "a1", UserRole = "teacher" },
//                new User { Id = 2, Name = "registered", UserName = "reg", Password = "reg", UserRole = "student" }
//            };
//            Course[] courses = new Course[]
//            {
//                new Course{ Id = 1, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0] },
//                new Course{ Id = 2, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0] }
//            };
//            CoursesListener[] listeners = new CoursesListener[]
//            {
//                new CoursesListener { Id = 2, Accepted = false, RequestedCourse = courses[1], Student = users[1] }
//            };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
//            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
//            mock.Setup(m => m.Users).Returns(users.AsQueryable());
//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = users[1].UserRole;
//            mockSession["id"] = users[1].Id.ToString();
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            string expected = "Ви вже подали заявку на курс, але викладач ще її не розглянув";
//            string actual = (controller.RequestCourse(2) as ViewResult).Model.ToString();
//            Assert.Equal(expected, actual);
//        }
//        [Fact]
//        public void NoStudentRequestCourseTest()
//        {
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockSession["id"] = "11";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            string expected = "Тільки студенти можуть подавати заявку на курс";
//            string actual = (controller.RequestCourse(2) as ViewResult).Model.ToString();
//            Assert.Equal(expected, actual);
//        }
//        [Fact]
//        public void NonAuthorizedRequestCourseTest()
//        {
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = null;
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            bool redirect = controller.RequestCourse(2) is RedirectToRouteResult;
//            Assert.True(redirect);
//        }
//        [Fact]
//        public void ViewCourseRequestsTest()
//        {
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User[] users = new User[]
//            {
//                new User { Id = 1, Name = "muzychuk", UserName = "anatoliy", Password = "a1", UserRole = "teacher" },
//                new User { Id = 2, Name = "registered", UserName = "reg", Password = "reg", UserRole = "student" }
//            };
//            Course[] courses = new Course[]
//            {
//                new Course{ Id = 1, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0] },
//                new Course{ Id = 2, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0] }
//            };
//            CoursesListener[] listeners = new CoursesListener[]
//            {
//                new CoursesListener { Id = 2, Accepted = false, RequestedCourse = courses[1], Student = users[1] }
//            };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
//            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
//            mock.Setup(m => m.Users).Returns(users.AsQueryable());
//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = users[0].UserRole;
//            mockSession["id"] = users[0].Id.ToString();
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            int expected = 1;
//            int actual = ((controller.ViewRequests() as ViewResult).Model as IEnumerable<CourseRequestModel>).Count();

//            Assert.Equal(expected, actual);
//        }
//        [Fact]
//        public void NoTeacherViewCourseRequestsTest()
//        {
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "student";
//            mockSession["id"] = "2";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            bool redirect = controller.ViewRequests() is RedirectToRouteResult;

//            Assert.True(redirect);
//        }
//        [Fact]
//        public void NotAuthorizedViewCourseRequestsTest()
//        {
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = null;
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            bool redirect = controller.ViewRequests() is RedirectToRouteResult;

//            Assert.True(redirect);
//        }
//        [Fact]
//        public void AcceptCourseRequestTest()
//        {
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
//            int stid = 1;
//            int cid = 1;

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            bool redirect = controller.AcceptCourse(stid, cid) is RedirectToRouteResult;
//        }
//        [Fact]
//        public void RefuseCourseRequestTest()
//        {
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();

//            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
//            MockHttpSession mockSession = new MockHttpSession();
//            mockSession["role"] = "teacher";
//            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
//            int stid = 1;
//            int cid = 1;

//            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
//            controller.ControllerContext.HttpContext = mockHttpContext.Object;

//            bool redirect = controller.RefuseCourse(stid, cid) is RedirectToRouteResult;
//        }
    }
}
