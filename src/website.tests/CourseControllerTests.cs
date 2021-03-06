﻿using System;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;

namespace website.tests
{
    public class CourseControllerTests
    {
        private readonly IQueryable<User> users;
        private Mock<Fake.FakeUserManager> userManager;
        private Mock<Fake.FakeSignInManager> signInManager;

        public CourseControllerTests()
        {
            users = new[] { new User() { UserName = "Bohdan" }, new User { UserName = "Modest" } }
                .AsQueryable();
            Fake.FakeIdentitySetuper.Setup(out userManager, out signInManager, users);
        }

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
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", PasswordHash = "123456" };
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
                    }, "Authentication"))
                }
            };

            //Act
            bool isCourseListener = (bool)(controller.ViewLesson(1) as ViewResult).ViewData["IsCourseListener"];

            //Assert
            Assert.False(isCourseListener);
        }

        [Fact]
        public void ViewLessonСourseListenerTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10")) { Id = 1 };

            Lesson lesson1 = new Lesson("Вступ у ASP .NET Core MVC", Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            pps.CourseLessons = new List<Lesson>();
            pps.CourseLessons.Add(lesson1);
            CoursesListener listener1 = new CoursesListener() { Id = 1, Student = student, RequestedCourse = pps, Accepted = true };

            Course[] courses = new Course[] { pps };
            CoursesListener[] listeners = new CoursesListener[] { listener1 };
            Lesson[] lessons = new Lesson[] { lesson1 };

            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());

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

            //Act
            bool isCourseListener = (bool)(controller.ViewLesson(1) as ViewResult).ViewData["IsCourseListener"];

            //Assert
            Assert.True(isCourseListener);
        }

        [Fact]
        public void ViewLessonСourseOwnerTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };

            Lesson lesson1 = new Lesson("Вступ у ASP .NET Core MVC", Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            pps.CourseLessons = new List<Lesson>();
            pps.CourseLessons.Add(lesson1);
            CoursesListener listener1 = new CoursesListener() { Id = 1, Student = student, RequestedCourse = pps, Accepted = true };

            Course[] courses = new Course[] { pps };
            CoursesListener[] listeners = new CoursesListener[] { listener1 };
            Lesson[] lessons = new Lesson[] { lesson1 };

            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());

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
            bool isOwner = (bool)(controller.ViewLesson(1) as ViewResult).ViewData["IsCourseListener"];

            //Assert
            Assert.True(isOwner);
        }

        [Fact]
        public void AddCourseGETGuestTry()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            Course[] courses = new Course[] { pps };
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
            //Act
            var result = controller.AddCourse();

            //Assert
            Assert.True(result is ViewResult);
        }

        [Fact]
        public void AddCourseGETTeacherTry()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            Course[] courses = new Course[] { pps };
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

            //Act & Assert
            Assert.True(controller.AddCourse() is ViewResult);
        }

        [Fact]
        public void AddCoursePOSTTeacher()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            Course[] courses = new Course[] { pps };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
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

            //Act & Assert
            string courseName = "Програмування мовою C#";
            ViewResult viewRes = (controller.AddCourse(courseName, "Опис", DateTime.Now, DateTime.Now).Result as ViewResult);
            string result = (string)viewRes.Model;
            Assert.True(result as string == String.Format("Курс \"{0}\" успішно створено!", courseName));
        }

        [Fact]
        public void AddLessonGETGuestTry()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            Course[] courses = new Course[] { pps };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
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

            //Act
            var result = controller.AddLesson(1).Result as ViewResult;

            //Assert
            Assert.True(result.Model == null);
        }

        [Fact]
        public void AddLessonGETTeacherNotOwnerTry()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User teacher1 = new User { Name = "Тарасюк С.І.", UserName = "tarasuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            Course[] courses = new Course[] { pps };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher1);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "tarasuk"),
                        new Claim(ClaimTypes.Role, "Teacher")
                    }, "Authentication"))
                }
            };

            //Act
            Task<IActionResult> result = controller.AddLesson(1);

            //Assert
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Home")) == true);
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "Index")) == true);
        }

        [Fact]
        public void AddLessonGETTeacherOwnerTry()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            Course[] courses = new Course[] { pps };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher);

            

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
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
            var result = controller.AddLesson(1).Result as ViewResult;

            //Assert
            Assert.True(result.Model == null);
        }

        [Fact]
        public void AddLessonPOSTTeacherOwnerTry()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            Course[] courses = new Course[] { pps };
            User[] users = new User[] { teacher };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
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

            string lessonName = "Вступ у мову програмування C#";
            List<IFormFile> files = new List<IFormFile>();
            HeaderDictionary dic = new HeaderDictionary(2);
            dic.Add(new KeyValuePair<string, StringValues>("Content-Disposition", new StringValues("form-data; name=\"files1\"; filename=\"test.txt\"")));
            dic.Add(new KeyValuePair<string, StringValues>("Content-Type", new StringValues("text/plain")));
            files.Add(new FormFile(File.Create(@"test.txt"), 2000, 0, "files1", "test.txt") { Headers = dic });
            //Act
            var result = controller.AddLesson(1, lessonName, "19.03.2018 11:50:00", "19.03.2018 11:50:00", "Опис уроку", "Опис домашки", "19.03.2018 11:50:00", "false", files, files);

            //Assert
            Assert.True((result.Result as ViewResult).Model as string == String.Format("Урок \"{0}\" успішно додано!", lessonName));
        }

        [Fact]
        public void AddHomeWorkPOSTStudentTry()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };

            Lesson pps = new Lesson("Шаблони", DateTime.Now, DateTime.Now, DateTime.Now) { Id = 1 };
            Lesson[] lessons = new Lesson[] { pps };
            User[] users = new User[] { teacher };
            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
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
            List<IFormFile> files = new List<IFormFile>();
            HeaderDictionary dic = new HeaderDictionary(2);
            dic.Add(new KeyValuePair<string, StringValues>("Content-Disposition", new StringValues("form-data; name=\"files1\"; filename=\"test.txt\"")));
            dic.Add(new KeyValuePair<string, StringValues>("Content-Type", new StringValues("text/plain")));
            files.Add(new FormFile(File.Create(@"test1.txt"), 2000, 0, "files", "test1.txt") { Headers = dic });

            //Act
            Task<IActionResult> result = controller.AddHomeWork(1, "Виконав пункт 1", files);

            //Assert
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Course")) == true);
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "ViewLesson")) == true);
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("id", 1)) == true);
        }

        [Fact]
        public void AddMarkByTeacher()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };

            Lesson pps = new Lesson("Шаблони", DateTime.Now, DateTime.Now, DateTime.Now) { Id = 1 };
            pps.Posts = new List<Post>();
            pps.Posts.Add(new Post("Пост", "homework", student, Convert.ToDateTime("2018-05-21 13:00")));
            Lesson[] lessons = new Lesson[] { pps };
            User[] users = new User[] { teacher };
            Post[] posts = new Post[] { new Post("Пост", "homework", student, Convert.ToDateTime("2018-05-21 13:00")) };
            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            mock.Setup(m => m.Posts).Returns(posts.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
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
            Task<IActionResult> result = controller.AddMark("5","1","1");

            //Assert
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Course")) == true);
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "ViewLesson")) == true);
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("id", 1)) == true);
        }

        [Fact]
        public void ViewLocationsCourseOwnerTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk", BirthYear=1997 };
            User student1 = new User { Name = "Радомський М.А.", UserName = "modest.radomskyy", BirthYear=1998 };

            Course[] courses = new Course[] { new Course("Проектування прорамних систем", "D", teacher, Convert.ToDateTime("2018-05-20 14:00"), Convert.ToDateTime("2019-05-20 14:00")) { Id = 1 } };
            User[] users = new User[] { teacher, student, student1 };
            CoursesListener[] listeners = new CoursesListener[] {
                new CoursesListener(1, student, courses[0]) { Accepted = true } ,
                new CoursesListener(2, student1, courses[0]) { Accepted = true }
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
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
            Task<IActionResult> result = controller.ViewLocations(1);

            //Assert
            Assert.True((result.Result as ViewResult).Model is  CourseStudentsModel &&
                ((result.Result as ViewResult).Model as CourseStudentsModel).Students.Count()==1);
        }

        [Fact]
        public void ViewLocationsNOTCourseOwnerTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher1 = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User teacher2 = new User { Name = "Радомський М.А.", UserName = "modest.radomskyy" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };

            Course[] courses = new Course[] { new Course("Проектування прорамних систем", "D", teacher1, Convert.ToDateTime("2018-05-20 14:00"), Convert.ToDateTime("2019-05-20 14:00")) { Id = 1 } };
            User[] users = new User[] { teacher1, teacher2, student };
            CoursesListener[] listeners = new CoursesListener[] { new CoursesListener(1, student, courses[0]) { Accepted = true } };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher2);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "modest.radomskyy"),
                        new Claim(ClaimTypes.Role, "Teacher")
                    }, "Authentication"))
                }
            };

            //Act
            Task<IActionResult> result = controller.ViewLocations(1);

            //Assert
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Home")) == true);
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "Index")) == true);
        }

        [Fact]
        public void ViewStudentsCourseOwnerTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };

            Course[] courses = new Course[] { new Course("Проектування прорамних систем", "D", teacher, Convert.ToDateTime("2018-05-20 14:00"), Convert.ToDateTime("2019-05-20 14:00")) { Id = 1 } };
            User[] users = new User[] { teacher, student };
            CoursesListener[] listeners = new CoursesListener[] { new CoursesListener(1, student, courses[0]) { Accepted = true } };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
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
            Task<IActionResult> result = controller.ViewStudents(1);

            //Assert
            Assert.True((result.Result as ViewResult).Model is CourseStudentsModel &&
                ((result.Result as ViewResult).Model as CourseStudentsModel).Students.Count() == 1);
        }

        [Fact]
        public void ViewStudentsNOTCourseOwnerTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher1 = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User teacher2 = new User { Name = "Радомський М.А.", UserName = "modest.radomskyy" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };

            Course[] courses = new Course[] { new Course("Проектування прорамних систем", "D", teacher1, Convert.ToDateTime("2018-05-20 14:00"), Convert.ToDateTime("2019-05-20 14:00")) { Id = 1 } };
            User[] users = new User[] { teacher1, teacher2, student };
            CoursesListener[] listeners = new CoursesListener[] { new CoursesListener(1, student, courses[0]) { Accepted = true } };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(teacher2);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "modest.radomskyy"),
                        new Claim(ClaimTypes.Role, "Teacher")
                    }, "Authentication"))
                }
            };

            //Act
            Task<IActionResult> result = controller.ViewStudents(1);

            //Assert
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("controller", "Home")) == true);
            Assert.True((result.Result as RedirectToRouteResult).RouteValues.Contains(new KeyValuePair<string, object>("action", "Index")) == true);
        }

        [Fact]
        public void RequestCourseTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User[] users = new User[]
            {
                        new User { Name = "muzychuk", UserName = "anatoliy", PasswordHash = "a1"},
                        new User { Name = "registered", UserName = "reg", PasswordHash = "reg"}
            };
            Course[] courses = new Course[]
            {
                        new Course{ Id = 1, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) },
                        new Course{ Id = 2, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) }
            };
            CoursesListener[] listeners = new CoursesListener[]
            {
                        new CoursesListener { Id = 2, Accepted = true, RequestedCourse = courses[0], Student = users[1] }
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());


            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                        new Claim(ClaimTypes.Name, "registered"),
                        new Claim(ClaimTypes.Role, "Student")
                   }, "Authentication"))
                }
            };

            string expected = "Ви подали заявку на курс, викладач розгляне її найближчим часом.";
            string actual = (controller.RequestCourse(2) as ViewResult).Model.ToString();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ExistingRequestCourseTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User[] users = new User[]
            {
                        new User { Name = "muzychuk", UserName = "anatoliy", PasswordHash = "a1"},
                        new User { Name = "registered", UserName = "registered", PasswordHash = "reg"}
            };
            Course[] courses = new Course[]
            {
                        new Course{ Id = 1, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) },
                        new Course{ Id = 2, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) }
            };
            CoursesListener[] listeners = new CoursesListener[]
            {
                        new CoursesListener { Id = 2, Accepted = false, RequestedCourse = courses[0], Student = users[1] }
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());


            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                        new Claim(ClaimTypes.Name, "registered"),
                        new Claim(ClaimTypes.Role, "Student")
                   }, "Authentication"))
                }
            };

            string expected = "Ви вже подали заявку на курс, але викладач ще її не розглянув";
            string actual = (controller.RequestCourse(1) as ViewResult).Model.ToString();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ViewCourseRequestsTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User[] users = new User[]
            {
                        new User { Name = "muzychuk", UserName = "anatoliy", PasswordHash = "a1"},
                        new User { Name = "registered", UserName = "registered", PasswordHash = "reg"}
            };
            Course[] courses = new Course[]
            {
                        new Course{ Id = 1, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) },
                        new Course{ Id = 2, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) }
            };
            CoursesListener[] listeners = new CoursesListener[]
            {
                        new CoursesListener { Id = 2, Accepted = false, RequestedCourse = courses[0], Student = users[1] }
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());


            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                        new Claim(ClaimTypes.Name, "anatoliy"),
                        new Claim(ClaimTypes.Role, "Teacher")
                   }, "Authentication"))
                }
            };
            int expected = 1;
            int actual = ((controller.ViewRequests() as ViewResult).Model as IEnumerable<CourseRequestModel>).Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AcceptCourseRequestTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User[] users = new User[]
            {
                        new User { Name = "muzychuk", UserName = "anatoliy", PasswordHash = "a1"},
                        new User { Name = "registered", UserName = "registered", PasswordHash = "reg"}
            };
            Course[] courses = new Course[]
            {
                        new Course{ Id = 1, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) },
                        new Course{ Id = 2, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) }
            };
            CoursesListener[] listeners = new CoursesListener[]
            {
                        new CoursesListener { Id = 2, Accepted = false, RequestedCourse = courses[0], Student = users[1] }
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());


            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                        new Claim(ClaimTypes.Name, "anatoliy"),
                        new Claim(ClaimTypes.Role, "Teacher")
                   }, "Authentication"))
                }
            };

            bool redirect = controller.AcceptCourse(users[1].UserName, courses[1].Id) is RedirectToRouteResult;
            Assert.True(redirect);
        }
        [Fact]
        public void RefuseCourseRequestTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User[] users = new User[]
            {
                        new User { Name = "muzychuk", UserName = "anatoliy", PasswordHash = "a1"},
                        new User { Name = "registered", UserName = "registered", PasswordHash = "reg"}
            };
            Course[] courses = new Course[]
            {
                        new Course{ Id = 1, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) },
                        new Course{ Id = 2, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) }
            };
            CoursesListener[] listeners = new CoursesListener[]
            {
                        new CoursesListener { Id = 2, Accepted = false, RequestedCourse = courses[0], Student = users[1] }
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());


            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                        new Claim(ClaimTypes.Name, "anatoliy"),
                        new Claim(ClaimTypes.Role, "Teacher")
                   }, "Authentication"))
                }
            };

            bool redirect = controller.RefuseCourse(users[1].UserName, courses[1].Id) is RedirectToRouteResult;
            Assert.True(redirect);
        }
        [Fact]
        public void DeleteFromCourseTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };
            
            CoursesListener listener1 = new CoursesListener() { Id = 1, Student = student, RequestedCourse = pps, Accepted = true };

            Course[] courses = new Course[] { pps };
            CoursesListener[] listeners = new CoursesListener[] { listener1 };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());

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

            //Act
            bool redirect = controller.DeleteFromCourse(1) is RedirectToRouteResult;

            //Assert
            Assert.True(redirect);
        }
        [Fact]
        public void DeleteFromCourseNotAcceptedTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };

            CoursesListener listener1 = new CoursesListener() { Id = 1, Student = student, RequestedCourse = pps, Accepted = false };

            Course[] courses = new Course[] { pps };
            CoursesListener[] listeners = new CoursesListener[] { listener1 };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());

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

            //Act
            string expected = "Ви ще не відвідуєте даний курс";
            string actual = (controller.DeleteFromCourse(1) as ViewResult).Model.ToString();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void DeleteFromCourseNotListenerTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            User student = new User { Name = "Романюк Богдан", UserName = "bohdan.romaniuk" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Id = 1 };

            Course[] courses = new Course[] { pps };
            CoursesListener[] listeners = new CoursesListener[] { };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());

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

            //Act
            string expected = "Ви не відвідуєте даний курс";
            string actual = (controller.DeleteFromCourse(1) as ViewResult).Model.ToString();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ViewListenersAgeStats()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User[] users = new User[]
            {
                        new User { Name = "muzychuk", UserName = "anatoliy", PasswordHash = "a1"},
                        new User { Name = "registered", UserName = "registered", PasswordHash = "reg", BirthYear = 1986},
                        new User { Name = "registered2", UserName = "registered2", PasswordHash = "reg", BirthYear = 1989},
                        new User { Name = "registered3", UserName = "registered3", PasswordHash = "reg", BirthYear = 1956}
            };
            Course[] courses = new Course[]
            {
                        new Course{ Id = 1, Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], StartDate = DateTime.Now.AddHours(1.0) },
            };
            CoursesListener[] listeners = new CoursesListener[]
            {
                        new CoursesListener { Id = 1, Accepted = true, RequestedCourse = courses[0], Student = users[1] },
                        new CoursesListener { Id = 2, Accepted = true, RequestedCourse = courses[0], Student = users[2] },
                        new CoursesListener { Id = 2, Accepted = true, RequestedCourse = courses[0], Student = users[3] }
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            
            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                        new Claim(ClaimTypes.Name, "anatoliy"),
                        new Claim(ClaimTypes.Role, "Teacher")
                   }, "Authentication"))
                }
            };

            int expected = 1989;
            int actual = Convert.ToInt32((controller.ViewListeners(courses[0].Id) as ViewResult).ViewData["MaxYear"]);
            Assert.Equal(expected, actual);
        }
    }
}
