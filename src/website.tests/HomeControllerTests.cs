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
        //[Fact]
        //public void CoursesList()
        //{
        //    //Arrange
        //    Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
        //    User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk"};
        //    Course[] courses = new Course[]
        //    {
        //        new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")),
        //        new Course("Програмування мовою с++", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10"))
        //    };
        //    mock.Setup(m => m.Courses).Returns(courses.AsQueryable());

        //    HomeController controller = new HomeController(mock.Object, null, null);
        //    controller.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = new DefaultHttpContext
        //        {
        //            User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //            {
        //                new Claim(ClaimTypes.Name, "anatoliy.muzychuk"),
        //                new Claim(ClaimTypes.Role, "Teacher")
        //            }, "Authentication"))
        //        }
        //    };

        //    //Act
        //    var result = (controller.Index() as ViewResult).Model as List<HomeViewModel>;

        //    //Assert
        //    Assert.Equal(courses.Length, result.Count());
        //}

        //[Fact]
        //public void CoursesSearch()
        //{
        //    //Arrange
        //    Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
        //    User teacher = new User { Name = "Музичук А.О.", UserName = "anatoliy.muzychuk" };
        //    Course[] courses = new Course[]
        //    {
        //        new Course("Проектування програмних систем", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")),
        //        new Course("Програмування мовою C++", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10")),
        //        new Course("Програмування мовою C#", "Опис курсу", teacher, Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10"))
        //    };
        //    mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
        //    HomeController controller = new HomeController(mock.Object, null, null);
        //    string searchCourseName = "Програмування";
        //    var expectedResult = courses.Where(c => c.Name.Contains(searchCourseName));
        //    controller.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = new DefaultHttpContext
        //        {
        //            User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //            {
        //                new Claim(ClaimTypes.Name, "anatoliy.muzychuk"),
        //                new Claim(ClaimTypes.Role, "Teacher")
        //            }, "Authentication"))
        //        }
        //    };

        //    //Act
        //    var result = (controller.Index(searchCourseName) as ViewResult).Model as List<HomeViewModel>;

        //    //Assert
        //    Assert.Equal(expectedResult.Count(), result.Count());
        //}
    }
}
