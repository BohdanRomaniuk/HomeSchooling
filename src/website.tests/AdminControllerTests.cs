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

namespace website.tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void CourseDeletionTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User[] users = new User[]
            {
                new User { Id = 1, Name = "muzychuk", UserName = "anatoliy", Password = "a1", UserRole = "teacher" },
                new User { Id = 2, Name = "registered", UserName = "reg", Password = "reg", UserRole = "student" }
            };
            Post[] posts = new Post[]
            {
                new Post{ Id = 1, PostedBy = users[0], PostType = "a", Text = "aaaa"}
            };
            Lesson[] lessons = new Lesson[]
            {
                new Lesson { Id = 1, IsControlWork = false, Name = "les1", Posts = posts.ToList()}
            };
            Course[] courses = new Course[]
            {
                new Course{ Name = "Проектування програмних систем", Description = "Опис курсу", Teacher = users[0], Id = 1, CourseLessons = lessons.ToList() }
            };
            CoursesListener[] listeners = new CoursesListener[]
            {
                new CoursesListener { Id = 2, Accepted = true, RequestedCourse = courses[0], Student = users[1] }
            };
            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
            mock.Setup(m => m.CoursesListeners).Returns(listeners.AsQueryable());
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["role"] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            AdminController controller = new AdminController(mock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            bool isredirect = controller.DeleteCourse(1) is RedirectToRouteResult;

            Assert.True(isredirect);
        }
        [Fact]
        public void SetTeacherTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User[] users = new User[]
            {
                new User { Id = 1, Name = "muzychuk", UserName = "anatoliy", Password = "a1", UserRole = "teacher" },
                new User { Id = 2, Name = "registered", UserName = "reg", Password = "reg", UserRole = "student" }
            };
            
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["role"] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            AdminController controller = new AdminController(mock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            bool isredirect = controller.SetTeacher(2) is RedirectToRouteResult;

            Assert.True(isredirect);
        }
    }
}
