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
        [Fact]
        public void LoginWrongPasswordTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User {Name = "a1", UserName = "a1", PasswordHash = "123456" };
            User[] users = new User[] { u1 };

            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            LoginModel lm = new LoginModel();
            lm.UserName = "new user";
            lm.Password = "123456";
            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Login(lm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void LoginWrongUsernameTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            User[] users = new User[] { u1 };

            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            LoginModel lm = new LoginModel();
            lm.UserName = "a1";
            lm.Password = "12345";
            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Login(lm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void LogingUserTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            User[] users = new User[] { u1 };

            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            LoginModel lm = new LoginModel();
            lm.UserName = "a1";
            lm.Password = "123456";
            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Login(lm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void LogingOutTest()
        {
            ProfileController controller = new ProfileController(null, null, signInManager.Object);
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
            bool redirected = (controller.Logout().Result is RedirectToActionResult);

            Assert.True(redirected);
        }
        [Fact]
        public void RegisterWrongUsernameTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            User[] users = new User[] { u1 };

            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            RegisterModel rm = new RegisterModel() { UserName = "a1", Password = "1", BirthYear = 1995, Email = "a@a.a", Location = "Stryi", Name = "a"};
     
            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Register(rm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void RegisterNullPasswordTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            User[] users = new User[] { u1 };

            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            RegisterModel rm = new RegisterModel() { UserName = "a1", Password = null, BirthYear = 1995, Email = "a@a.a", Location = "Stryi", Name = "a" };

            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Register(rm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void RegisterNullNameTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            User[] users = new User[] { u1 };

            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            RegisterModel rm = new RegisterModel() { UserName = null, Password = "1", BirthYear = 1995, Email = "a@a.a", Location = "Stryi", Name = "a" };

            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Register(rm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void RegisterWrongYearTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            User[] users = new User[] { u1 };

            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            RegisterModel rm = new RegisterModel() { UserName = "a", Password = "1", BirthYear = 1, Email = "a@a.a", Location = "Stryi", Name = "a" };

            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Register(rm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void RegisterTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            User[] users = new User[] { u1 };

            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            RegisterModel rm = new RegisterModel() { UserName = "a", Password = "1", BirthYear = 1995, Email = "a@a.a", Location = "Stryi", Name = "a" };

            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Register(rm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void ViewProfileStudentTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User student = new User { Name = "Бучелла", UserName = "buchyk", PasswordHash = "123456" };
            User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", PasswordHash = "123456" };
            Course ppc = new Course("Програмування мовою с++", "Опис курсу", teacher, Convert.ToDateTime("12.12.2018 19:00"), Convert.ToDateTime("12.05.2019 18:00")) { Id = 2 };
            ppc.CourseLessons = new List<Lesson>();
            ppc.CourseLessons.Add(new Lesson("Вступ у мову програмування C++", Convert.ToDateTime("01.09.2017 08:30"), Convert.ToDateTime("01.09.2017 09:50"), Convert.ToDateTime("08.09.2017 09:50")));
            ppc.CourseLessons.Add(new Lesson("Типи змінних, синтаксис", Convert.ToDateTime("08.09.2017 08:30"), Convert.ToDateTime("08.09.2017 09:50"), Convert.ToDateTime("15.09.2017 09:50")));
            Course[] courses = new Course[] { ppc };
            List<User> users = new List<User>();
            List<CoursesListener> listeners = new List<CoursesListener>() { new CoursesListener() { Id = 1, RequestedCourse = ppc, Student = student, Accepted = true } };
            users.Add(student);
            List<string> roles = new List<string>();
            roles.Add("Student");
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
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

            int expected = 1;
            int actual = ((controller.View("buchyk").Result as ViewResult).Model as IEnumerable<Course>).Count();

            Assert.Equal(expected, actual);
        }
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
            List<User> users = new List<User>();
            users.Add(teacher);
            List<string> roles = new List<string>();
            roles.Add("Teacher");
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
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
