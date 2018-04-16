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

namespace website.tests
{
    public class CourseControllerTests
    {
        [Fact]
        public void ViewCourseStudentTesting()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
            Course ppc = new Course("Програмування мовою с++", "Опис курсу", teacher) { Id = 2 };
            pps.CourseLessons = new List<Lesson>();
            ppc.CourseLessons = new List<Lesson>();
            pps.CourseLessons.Add(new Lesson("Вступ у ASP .NET Core MVC", Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")));
            pps.CourseLessons.Add(new Lesson("Use Case and Domain models", Convert.ToDateTime("12.03.2018 11:50"), Convert.ToDateTime("12.03.2018 13:10"), Convert.ToDateTime("19.03.2018 13:10")));
            pps.CourseLessons.Add(new Lesson("Побудова підсистем", Convert.ToDateTime("19.03.2018 11:50"), Convert.ToDateTime("19.03.2018 13:10"), Convert.ToDateTime("27.03.2018 13:10")));
            ppc.CourseLessons.Add(new Lesson("Вступ у мову програмування C++", Convert.ToDateTime("01.09.2017 08:30"), Convert.ToDateTime("01.09.2017 09:50"), Convert.ToDateTime("08.09.2017 09:50")));
            ppc.CourseLessons.Add(new Lesson("Типи змінних, синтаксис", Convert.ToDateTime("08.09.2017 08:30"), Convert.ToDateTime("08.09.2017 09:50"), Convert.ToDateTime("15.09.2017 09:50")));
            Course[] courses = new Course[] { pps, ppc };

            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["role"] = "student";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

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
            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
            Course ppc = new Course("Програмування мовою с++", "Опис курсу", teacher) { Id = 2 };
            pps.CourseLessons = new List<Lesson>();
            ppc.CourseLessons = new List<Lesson>();
            pps.CourseLessons.Add(new Lesson("Вступ у ASP .NET Core MVC", Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")));
            pps.CourseLessons.Add(new Lesson("Use Case and Domain models", Convert.ToDateTime("12.03.2018 11:50"), Convert.ToDateTime("12.03.2018 13:10"), Convert.ToDateTime("19.03.2018 13:10")));
            pps.CourseLessons.Add(new Lesson("Побудова підсистем", Convert.ToDateTime("19.03.2018 11:50"), Convert.ToDateTime("19.03.2018 13:10"), Convert.ToDateTime("27.03.2018 13:10")));
            ppc.CourseLessons.Add(new Lesson("Вступ у мову програмування C++", Convert.ToDateTime("01.09.2017 08:30"), Convert.ToDateTime("01.09.2017 09:50"), Convert.ToDateTime("08.09.2017 09:50")));
            ppc.CourseLessons.Add(new Lesson("Типи змінних, синтаксис", Convert.ToDateTime("08.09.2017 08:30"), Convert.ToDateTime("08.09.2017 09:50"), Convert.ToDateTime("15.09.2017 09:50")));
            Course[] courses = new Course[] { pps, ppc };

            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["role"] = "teacher";
            mockSession["id"] = "1";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

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

            Lesson pps1 = new Lesson() { Id = 1, Name = "Вступ у ASP.NET Core MVC", LessonStartDate = Convert.ToDateTime("05.03.2018 11:50"), LessonEndDate = Convert.ToDateTime("05.03.2018 13:10"), HomeWorkEnd = Convert.ToDateTime("12.03.2018 13:10"), IsControlWork=false };
            Lesson pps2 = new Lesson() { Id = 2, Name = "Побудова підсистем", LessonStartDate = Convert.ToDateTime("05.03.2018 11:50"), LessonEndDate = Convert.ToDateTime("05.03.2018 13:10"), HomeWorkEnd = Convert.ToDateTime("12.03.2018 13:10"), IsControlWork=false };
            Lesson[] lessons = new Lesson[] { pps1, pps2};

            mock.Setup(m => m.Lessons).Returns(lessons.AsQueryable());
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["role"] = null;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

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
            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
            User student = new User { Id = 2, Name = "Романюк Богдан", UserName = "bohdan.romaniuk", Password = "123456", UserRole = "student" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
            
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

            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["role"] = "student";
            mockSession["id"] = "2";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

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
            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
            User student = new User { Id = 2, Name = "Романюк Богдан", UserName = "bohdan.romaniuk", Password = "123456", UserRole = "student" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };

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

            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["role"] = "teacher";
            mockSession["id"] = "1";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            CourseController controller = new CourseController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            bool isOwner = (bool)(controller.ViewLesson(1) as ViewResult).ViewData["IsCourseListener"];

            //Assert
            Assert.True(isOwner);
        }

        [Fact]
        public void AddCourseGuest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
            Course pps = new Course("Проектування програмних систем", "Опис курсу", teacher) { Id = 1 };
            Course[] courses = new Course[] { pps };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
        }
    }
}
