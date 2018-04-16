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
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace website.tests
{
    public class ProfileControllerTests
    {
        [Fact]
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
        }
        [Fact]
        public void LoginWrongUsernameTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User { Id = 1, Name = "a1", UserName = "a1", Password = "a1", UserRole = "student" };
            User u2 = new User { Id = 2, Name = "a2", UserName = "a2", Password = "a2", UserRole = "student" };
            User u3 = new User { Id = 3, Name = "a3", UserName = "a3", Password = "a3", UserRole = "student" };
            User[] users = new User[] { u1, u3 };
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            ProfileController controller = new ProfileController(mock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            bool actual = (bool)(controller.Login(u2) as ViewResult).ViewData["IncorrectLogin"];

            Assert.True(actual);
        }
        [Fact]
        public void LogingUserTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            User u1 = new User { Id = 1, Name = "a1", UserName = "a1", Password = "a1", UserRole = "student" };
            User u2 = new User { Id = 2, Name = "a2", UserName = "a2", Password = "a2", UserRole = "student" };
            User u3 = new User { Id = 3, Name = "a3", UserName = "a3", Password = "a3", UserRole = "student" };
            User[] users = new User[] { u1, u2, u3 };
            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);

            ProfileController controller = new ProfileController(mock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            bool redirected = (controller.Login(u2) is RedirectToRouteResult);

            Assert.True(redirected);
        }
        [Fact]
        public void LogingOutTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession["id"] = "1";
            mockSession["username"] = "a";
            mockSession["role"] = "teacher";
            mockSession["name"] = "misha";

            ProfileController controller = new ProfileController(mock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            bool redirected = (controller.Logout() is RedirectToRouteResult);

            Assert.True(redirected);
        }
    }
}
