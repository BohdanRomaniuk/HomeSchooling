using database.Models;
using website;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using website.Controllers;
using website.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace website.tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void AllCoursesListTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course[] courses = new Course[]
            {
                new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Category = "1"},
                new Course("Програмування мовою с++", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Category = "1"}
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

            HomeController controller = new HomeController(mock.Object, null, null);
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
            var result = ((controller.Index() as ViewResult).Model as HomeCoursesViewModel).Courses;

            //Assert
            Assert.Equal(courses.Length, result.Count());
        }

        [Fact]
        public void AllCoursesListForStudentTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course[] courses = new Course[]
            {
                new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Category = "1", Id=1},
                new Course("Програмування мовою с++", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Category = "1", Id=2}
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

            HomeController controller = new HomeController(mock.Object, null, null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "modest"),
                        new Claim(ClaimTypes.Role, "Student")
                    }, "Authentication"))
                }
            };

            //Act
            var result = ((controller.Index() as ViewResult).Model as HomeCoursesViewModel).Courses;

            //Assert
            Assert.Equal(courses.Length, result.Count());
        }

        [Fact]
        public void CategoryCoursesListTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course[] courses = new Course[]
            {
                new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Category = "1"},
                new Course("Програмування мовою с++", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Category = "2"}
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

            HomeController controller = new HomeController(mock.Object, null, null);
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
            var result = ((controller.Index(category:"1") as ViewResult).Model as HomeCoursesViewModel).Courses;
            var expected = 1;

            //Assert
            Assert.Equal(expected, result.Count());
        }
        [Fact]
        public void SearchCoursesListTest()
        {
            //Arrange
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
            Course[] courses = new Course[]
            {
                new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Category = "1"},
                new Course("Програмування мовою с++", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")) { Category = "2"}
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

            HomeController controller = new HomeController(mock.Object, null, null);
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
            var result = ((controller.Index(name: "Проектування") as ViewResult).Model as HomeCoursesViewModel).Courses;
            var expected = 1;

            //Assert
            Assert.Equal(expected, result.Count());
        }
    }
}
