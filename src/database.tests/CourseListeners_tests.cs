using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using database.Models;

namespace database.tests
{
   public class CourseListeners_tests
    {
        [Fact]
        public void Constructor_test()
        {
            int id = 1;
            User test_user = new User();
            Course test_course = new Course();
            CoursesListener test_obj = new CoursesListener(id,test_user,test_course);
            Assert.Equal(id, test_obj.Id);
            Assert.Equal(test_user, test_obj.Student);
            Assert.Equal(test_course, test_obj.RequestedCourse);
        }
        [Fact]
        public void GetSet_test()
        {
            int id = 1;
            User test_user = new User();
            Course test_course = new Course();
            CoursesListener test_obj = new CoursesListener();
            test_obj.Id = id;
           test_obj.Student = test_user;
            test_obj.RequestedCourse = test_course;
           
            Assert.Equal(id, test_obj.Id);
            Assert.Equal(test_user, test_obj.Student);
            Assert.Equal(test_course, test_obj.RequestedCourse);
        }
    }
}
