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
using Microsoft.AspNetCore.Identity;

namespace website.tests
{
    public class ProfileControllerTests
    {
        private readonly IQueryable<User> users;
        private Mock<Fake.FakeUserManager> userManager;
        private Mock<Fake.FakeSignInManager> signInManager;

        public ProfileControllerTests()
        {
            users = new[] { new User() { UserName = "Bohdan" }, new User { UserName = "Modest" } }
                .AsQueryable();
            Fake.FakeIdentitySetuper.Setup(out userManager, out signInManager, users);
        }
        /* [Fact]
         public void LoginWrongPasswordTest()
         {
             Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
             User u1 = new User { Id = 1, Name = "a1", UserName = "a1", Password = "a1", UserRole = "student" };
             User u2 = new User { Id = 2, Name = "a1", UserName = "a1", Password = "raz", UserRole = "student" };
             User u3 = new User { Id = 3, Name = "a3", UserName = "a3", Password = "a3", UserRole = "student" };
             User[] users = new User[] { u1, u3 };
             mock.Setup(m => m.Users).Returns(users.AsQueryable());
             Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
             MockHttpSession mockSession = new MockHttpSession();
             mockHttpContext.Setup(s => s.Session).Returns(mockSession);

             ProfileController controller = new ProfileController(mock.Object);
             controller.ControllerContext.HttpContext = mockHttpContext.Object;

             bool actual = (bool)(controller.Login(u2) as ViewResult).ViewData["IncorrectPassword"];

             Assert.True(actual);
         }*/
        //        [Fact]
        //        public void LoginWrongUsernameTest()
        //        {
        //            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
        //            User u1 = new User { Id = 1, Name = "a1", UserName = "a1", Password = "a1", UserRole = "student" };
        //            User u2 = new User { Id = 2, Name = "a2", UserName = "a2", Password = "a2", UserRole = "student" };
        //            User u3 = new User { Id = 3, Name = "a3", UserName = "a3", Password = "a3", UserRole = "student" };
        //            User[] users = new User[] { u1, u3 };
        //            mock.Setup(m => m.Users).Returns(users.AsQueryable());
        //            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        //            MockHttpSession mockSession = new MockHttpSession();
        //            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

        //            ProfileController controller = new ProfileController(mock.Object);
        //            controller.ControllerContext.HttpContext = mockHttpContext.Object;

        //            bool actual = (bool)(controller.Login(u2) as ViewResult).ViewData["IncorrectLogin"];

        //            Assert.True(actual);
        //        }
        //        [Fact]
        //        public void LogingUserTest()
        //        {
        //            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
        //            User u1 = new User { Id = 1, Name = "a1", UserName = "a1", Password = "a1", UserRole = "student" };
        //            User u2 = new User { Id = 2, Name = "a2", UserName = "a2", Password = "a2", UserRole = "student" };
        //            User u3 = new User { Id = 3, Name = "a3", UserName = "a3", Password = "a3", UserRole = "student" };
        //            User[] users = new User[] { u1, u2, u3 };
        //            mock.Setup(m => m.Users).Returns(users.AsQueryable());
        //            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        //            MockHttpSession mockSession = new MockHttpSession();
        //            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

        //            ProfileController controller = new ProfileController(mock.Object);
        //            controller.ControllerContext.HttpContext = mockHttpContext.Object;

        //            bool redirected = (controller.Login(u2) is RedirectToRouteResult);

        //            Assert.True(redirected);
        //        }
        //        [Fact]
        //        public void LogingOutTest()
        //        {
        //            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();

        //            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        //            MockHttpSession mockSession = new MockHttpSession();
        //            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        //            mockSession["id"] = "1";
        //            mockSession["username"] = "a";
        //            mockSession["role"] = "teacher";
        //            mockSession["name"] = "misha";

        //            ProfileController controller = new ProfileController(mock.Object);
        //            controller.ControllerContext.HttpContext = mockHttpContext.Object;

        //            bool redirected = (controller.Logout() is RedirectToRouteResult);

        //            Assert.True(redirected);
        //        }
        //        [Fact]
        //        public void RegisterWrongUsernameTest()
        //        {
        //            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
        //            User u1 = new User { Id = 1, Name = "a1", UserName = "a1", Password = "a1", UserRole = "student" };
        //            User u2 = new User { Id = 2, Name = "a1", UserName = "a1", Password = "a2", UserRole = "student" };
        //            User u3 = new User { Id = 3, Name = "a3", UserName = "a3", Password = "a3", UserRole = "student" };
        //            User[] users = new User[] { u1, u3 };
        //            mock.Setup(m => m.Users).Returns(users.AsQueryable());
        //            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        //            MockHttpSession mockSession = new MockHttpSession();
        //            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

        //            ProfileController controller = new ProfileController(mock.Object);
        //            controller.ControllerContext.HttpContext = mockHttpContext.Object;

        //            bool actual = (bool)(controller.Register(u2) as ViewResult).ViewData["IncorrectLogin"];

        //            Assert.True(actual);
        //        }
        //        [Fact]
        //        public void RegisterNullPasswordTest()
        //        {
        //            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
        //            User u1 = new User { Id = 1, Name = "a1", UserName = "a1", Password = "a1", UserRole = "student" };
        //            User u2 = new User { Id = 2, Name = "a2", UserName = "a2", Password = null, UserRole = "student" };
        //            User u3 = new User { Id = 3, Name = "a3", UserName = "a3", Password = "a3", UserRole = "student" };
        //            User[] users = new User[] { u1, u3 };
        //            mock.Setup(m => m.Users).Returns(users.AsQueryable());
        //            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        //            MockHttpSession mockSession = new MockHttpSession();
        //            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

        //            ProfileController controller = new ProfileController(mock.Object);
        //            controller.ControllerContext.HttpContext = mockHttpContext.Object;

        //            bool actual = (bool)(controller.Register(u2) as ViewResult).ViewData["IncorrectPassword"];

        //            Assert.True(actual);
        //        }
        //        [Fact]
        //        public void RegisterNullNameTest()
        //        {
        //            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
        //            User u1 = new User { Id = 1, Name = "a1", UserName = "a1", Password = "a1", UserRole = "student" };
        //            User u2 = new User { Id = 2, Name = null, UserName = "a2", Password = "a2", UserRole = "student" };
        //            User u3 = new User { Id = 3, Name = "a3", UserName = "a3", Password = "a3", UserRole = "student" };
        //            User[] users = new User[] { u1, u3 };
        //            mock.Setup(m => m.Users).Returns(users.AsQueryable());
        //            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        //            MockHttpSession mockSession = new MockHttpSession();
        //            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

        //            ProfileController controller = new ProfileController(mock.Object);
        //            controller.ControllerContext.HttpContext = mockHttpContext.Object;

        //            bool actual = (bool)(controller.Register(u2) as ViewResult).ViewData["IncorrectName"];

        //            Assert.True(actual);
        //        }

        //        [Fact]
        //        public void ViewProfileStudentTest()
        //        {
        //            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
        //            User u1 = new User { Id = 1, Name = "a1", UserName = "a1", Password = "a1", UserRole = "student" };
        //            User u2 = new User { Id = 2, Name = "registered", UserName = "reg", Password = "reg", UserRole = "student" };
        //            User[] users = new User[]
        //            {
        //                new User { Id = 1, Name = "muzychuk", UserName = "anatoliy", Password = "a1", UserRole = "teacher" },
        //                new User { Id = 2, Name = "registered", UserName = "reg", Password = "reg", UserRole = "student" }
        //            };
        //            Course[] courses = new Course[]
        //            {
        //                new Course("Проектування програмних систем", "Опис курсу", users[0]),
        //                new Course("Програмування мовою с++", "Опис курсу", users[0])
        //            };
        //            CoursesListener[] listeners = new CoursesListener[]
        //            {
        //                new CoursesListener { Id = 2, Accepted = true, RequestedCourse = courses[1], Student = users[1] }
        //            };
        //            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
        //            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
        //            mock.Setup(m => m.Users).Returns(users.AsQueryable());
        //            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        //            MockHttpSession mockSession = new MockHttpSession();
        //            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

        //            ProfileController controller = new ProfileController(mock.Object);
        //            controller.ControllerContext.HttpContext = mockHttpContext.Object;

        //            int expected = 2;
        //            int actual = ((controller.View(2) as ViewResult).Model as IEnumerable<Course>).Count();

        //            Assert.Equal(expected, actual);
        //        }
        [Fact]
        public void ViewProfileTeacherTest()
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
            List<string> roles = new List<string>();
            roles.Add("Teacher");
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);

            ProfileController controller = new ProfileController(mock.Object, userManager.Object, null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "romaniuk.bohdan"),
                        new Claim(ClaimTypes.Role, "Student")
                    }, "Authentication"))
                }
            };

            int expected = 2;
            int actual = ((controller.View("anatoliy.muzychuk").Result as ViewResult).Model as IEnumerable<Course>).Count();

            Assert.Equal(expected, actual);
        }
    }   
}
