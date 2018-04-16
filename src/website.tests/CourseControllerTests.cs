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
    }
}
