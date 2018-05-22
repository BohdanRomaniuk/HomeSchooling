using database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using website.Models;
using Xunit;
using Moq;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace website.tests
{
    public class HomeSchoolingRepositoryTests
    {
        [Fact]
        public void AddCourseTest()
        {
            //Mock<HomeSchoolingContext> mock = new Mock<HomeSchoolingContext>();
            //Course[] courses = new Course[] 
            //{
            //    new Course("Проектування програмних систем", "Опис курсу", null, Convert.ToDateTime("12.12.2018 19:00"), Convert.ToDateTime("12.05.2019 18:00")) { Id = 1 },
            //    new Course("Програмування мовою с++", "Опис курсу", null, Convert.ToDateTime("12.12.2018 19:00"), Convert.ToDateTime("12.05.2019 18:00")) { Id = 2 }
            //};
            //mock.Setup(m => m.Courses).Returns((DbSet<Course>)courses.AsQueryable());
            //HomeSchoolingRepository repo = new HomeSchoolingRepository(mock.Object);
            //repo.AddCourse(new Course("БД", "БД", null, DateTime.Now, DateTime.Now));
            //Assert.Equal(3, repo.Courses.Count());
        }
    }
}
