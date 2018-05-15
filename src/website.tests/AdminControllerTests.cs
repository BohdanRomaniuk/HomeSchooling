using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using website.Models;
using database.Models;
using website.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace website.tests
{
    public class AdminControllerTests
    {
        private readonly IQueryable<User> users;
        private Mock<Fake.FakeUserManager> userManager;
        private Mock<Fake.FakeSignInManager> signInManager;

        public AdminControllerTests()
        {
            users = new[] { new User() { UserName = "Bohdan" }, new User { UserName = "Modest" } }
                .AsQueryable();
            Fake.FakeIdentitySetuper.Setup(out userManager, out signInManager, users);
        }
        [Fact]
        public void CourseDeletionTest()
        {
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

            AdminController controller = new AdminController(mock.Object, null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Role, "Admin")
                    }, "Authentication"))
                }
            };

            bool isredirect = controller.DeleteCourse(1) is RedirectToRouteResult;

            Assert.True(isredirect);
        }
        [Fact]
        public void SetTeacherTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            List<User> users = new List<User>();
            User student = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", PasswordHash = "123456"};
            users.Add(student);

            mock.Setup(m => m.Users).Returns(users.AsQueryable());

            AdminController controller = new AdminController(mock.Object, userManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Role, "Admin")
                    }, "Authentication"))
                }
            };

            bool result = controller.SetTeacher("anatoliy.muzychuk").Result is RedirectToRouteResult;

            Assert.True(result);
        }
    }
}
