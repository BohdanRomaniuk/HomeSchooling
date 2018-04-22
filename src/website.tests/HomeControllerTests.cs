//using database.Models;
//using website;
//using Xunit;
//using Moq;
//using System.Collections.Generic;
//using System.Linq;
//using website.Controllers;
//using website.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using Microsoft.AspNetCore.Mvc;

//namespace website.tests
//{
//    public class HomeControllerTests
//    {
//        [Fact]
//        public void CoursesList()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course[] courses = new Course[]
//            {
//                new Course("Проектування програмних систем", "Опис курсу", teacher),
//                new Course("Програмування мовою с++", "Опис курсу", teacher)
//            };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
//            HomeController controller = new HomeController(mock.Object);
            
//            //Act
//            var result = (controller.Index() as ViewResult).Model as IQueryable<Course>;

//            //Assert
//            Assert.Equal(courses.Length, result.Count());
//        }

//        [Fact]
//        public void CoursesSearch()
//        {
//            //Arrange
//            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
//            User teacher = new User { Id = 1, Name = "Музичук А.О.", UserName = "anatoliy.muzychuk", Password = "123456", UserRole = "teacher" };
//            Course[] courses = new Course[]
//            {
//                new Course("Проектування програмних систем", "Опис курсу", teacher),
//                new Course("Програмування мовою C++", "Опис курсу", teacher),
//                new Course("Програмування мовою C#", "Опис курсу", teacher)
//            };
//            mock.Setup(m => m.Courses).Returns(courses.AsQueryable());
//            HomeController controller = new HomeController(mock.Object);
//            string searchCourseName = "Програмування";
//            var expectedResult = courses.Where(c => c.Name.Contains(searchCourseName));

//            //Act
//            var result = (controller.Index(searchCourseName) as ViewResult).Model as IQueryable<Course>;

//            //Assert
//            Assert.Equal(expectedResult.Count(), result.Count());
//        }
//    }
//}
