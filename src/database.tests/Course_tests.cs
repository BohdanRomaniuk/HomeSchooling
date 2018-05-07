using Xunit;
using database.Models;
using System;

namespace database.tests
{
    public class Course_tests
    {
        [Fact]
        public void Set_test()
        {
            Course test_obj = new Course();
            test_obj.Name = "text1";
            test_obj.Description = "text2";
            test_obj.Teacher.Name = "text3";
            test_obj.StartDate = DateTime.Today;
            test_obj.EndDate = DateTime.Today;

            Assert.Equal("text1", test_obj.Name);
            Assert.Equal("text2", test_obj.Description);
            Assert.Equal("text3", test_obj.Teacher.Name);
            Assert.Equal(DateTime.Today, test_obj.StartDate);
            Assert.Equal(DateTime.Today, test_obj.EndDate);
        }

        [Fact]
        public void Constructor_test()
        {
            
            string name = "text1";
            string description = "text2";
            string teacher_Name = "text3";
            User teacher = new User();
            teacher.Name = teacher_Name;
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            Course test_obj = new Course(name,description,teacher,startDate,endDate);
            Assert.Equal("text1", test_obj.Name);
            Assert.Equal("text2", test_obj.Description);
            Assert.Equal("text3", test_obj.Teacher.Name);
            Assert.Equal(DateTime.Today, test_obj.StartDate);
            Assert.Equal(DateTime.Today, test_obj.EndDate);
        }
    }
}
