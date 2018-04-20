using System;
using System.Collections.Generic;
using System.Text;
using website.Models;
using Xunit;

namespace website.tests
{
    public class CourseRequestModelTests
    {
        [Fact]
        public void CourseRequestModelTest()
        {
            //ArrangeModest
            int field1 = 1;
            int field2 = 2;
            string field_3 = "text1";
            string field_4 = "text2";
            string field_5 = "text3";
            CourseRequestModel testobj1 = new CourseRequestModel(1, 2, "text1", "text2", "text3");

            //Act
            CourseRequestModel testobj2 = new CourseRequestModel(field1, field2, field_3, field_4,field_5);
            //Assert
            Assert.Equal(testobj1.CourseId, testobj2.CourseId);
            Assert.Equal(testobj1.CourseName, testobj2.CourseName);
            Assert.Equal(testobj1.StudentId, testobj2.StudentId);
            Assert.Equal(testobj1.StudentName, testobj2.StudentName);
            Assert.Equal(testobj1.StudentUserName, testobj2.StudentUserName);

        }
        [Fact]
        public void CourseIdTest()
        {
            //Arrange
            CourseRequestModel testobj = new CourseRequestModel(1,1,"text","text","text");
            int testId = 2;

            //Act
            testobj.CourseId = testId;
            //Assert
            Assert.Equal(testId, testobj.CourseId);

        }
        [Fact]
        public void CourseNameTest()
        {
            //Arrange
            CourseRequestModel testobj = new CourseRequestModel(1, 1, "text", "text", "text");
            string testName = "Text1";

            //Act
            testobj.CourseName = testName;
            //Assert
            Assert.Equal(testName, testobj.CourseName);

        }
        [Fact]
        public void StudentIdTest()
        {
            //Arrange
            CourseRequestModel testobj = new CourseRequestModel(1, 1, "text", "text", "text");
            int testId = 2;

            //Act
            testobj.StudentId = testId;
            //Assert
            Assert.Equal(testId, testobj.StudentId);

        }
        [Fact]
        public void StudentNameTest()
        {
            //Arrange
            CourseRequestModel testobj = new CourseRequestModel(1, 1, "text", "text", "text");
            string testName = "text1";

            //Act
            testobj.StudentName = testName;
            //Assert
            Assert.Equal(testName, testobj.StudentName);

        }
        [Fact]
        public void StudentUserNameTest()
        {
            //Arrange
            CourseRequestModel testobj = new CourseRequestModel(1, 1, "text", "text", "text");
            string testName = "text1";

            //Act
            testobj.StudentUserName = testName;
            //Assert
            Assert.Equal(testName, testobj.StudentUserName);

        }
    }
}
