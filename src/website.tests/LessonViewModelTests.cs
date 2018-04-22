//using database.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using website.Models;
//using Xunit;

//namespace website.tests
//{
//    public class LessonViewModelTests
//    {

//        [Fact]
//        public void LessonViewModelTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            User testuser = new User();
//            for (int i = 0; i < 20; ++i)
//            {
//                Post testpost = new Post("text1" + i, "homework", testuser, DateTime.Today);
//                testcurrentlesson.Posts.Add(testpost);
//            }
//            testcurrentlesson.Posts.Add(new Post("Lesson1", "lesson-desc", testuser, DateTime.Now));
//            testcurrentlesson.Posts.Add(new Post("Homework", "homework-desc", testuser, DateTime.Now));

//            //Act
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            //Assert
//            Assert.Equal(testcurrentlesson.Id,testobj1.LessonId);
//            Assert.Equal(testcurrentlesson.Name, testobj1.LessonName);
//            Assert.Equal(testcurrentlesson.LessonStartDate, testobj1.LessonStartDate);
//            Assert.Equal(testcurrentlesson.LessonEndDate, testobj1.LessonEndDate);
//            Assert.Equal(testcurrentlesson.HomeWorkEnd, testobj1.HomeWorkEnd);

//        }
//        [Fact]
//        public void LessonIdTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            int testId = 1;

//            //Act
//            testobj1.LessonId = testId;
//            //Assert
//            Assert.Equal(testobj1.LessonId, testId);

//        }

//        [Fact]
//        public void LessonNameTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            string testName = "test";

//            //Act
//            testobj1.LessonName = testName;
//            //Assert
//            Assert.Equal(testobj1.LessonName, testName);

//        }
//        [Fact]
//        public void LessonDescriptionTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            User testuser = new User();
//            Post testpost = new Post("text1","text2",testuser,DateTime.Today);
//            //Act
//            testobj1.LessonDescription = testpost;
//            //Assert
//            Assert.Equal(testobj1.LessonDescription, testpost);

//        }
       
//        [Fact]
//        public void HomeWorkDescriptionTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            User testuser = new User();
//            Post testpost = new Post("text1", "text2", testuser, DateTime.Today);
//            //Act
//            testobj1.HomeWorkDescription = testpost;
//            //Assert
//            Assert.Equal(testobj1.HomeWorkDescription, testpost);

//        }
//        [Fact]
//        public void LessonStartDateTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            DateTime testDate = new DateTime(2018, 12, 4, 13, 30, 50);
//            //Act
//            testobj1.LessonStartDate = testDate;
//            //Assert
//            Assert.Equal(testobj1.LessonStartDate, testDate);

//        }
//        [Fact]
//        public void LessonEndDateTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            DateTime testDate = new DateTime(2018, 12, 4, 13, 30, 50);
//            //Act
//            testobj1.LessonEndDate = testDate;
//            //Assert
//            Assert.Equal(testobj1.LessonEndDate, testDate);

//        }

//        [Fact]
//        public void HomeWorkEndTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            DateTime testDate = new DateTime(2018, 12, 4, 13, 30, 50);
//            //Act
//            testobj1.HomeWorkEnd = testDate;
//            //Assert
//            Assert.Equal(testobj1.HomeWorkEnd, testDate);

//        }
//        [Fact]
//        public void HomeWorksTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            List<Post> testlist = new List<Post>();
//            for(int i=0; i<20;++i)
//            {
//                User testuser = new User();
//                Post testpost = new Post("text1"+i, "text2"+i, testuser, DateTime.Today);
//                testlist.Add(testpost);
//            }
           
//            //Act
//            testobj1.HomeWorks = testlist;
//            //Assert
//            Assert.Equal(testobj1.HomeWorks, testlist);

//        }
//        [Fact]
//        public void IsControlWorkTest()
//        {
//            //Arrange
//            Lesson testcurrentlesson = new Lesson("text1", DateTime.Today, DateTime.Today, DateTime.Today, false);
//            LessonViewModel testobj1 = new LessonViewModel(testcurrentlesson);
//            bool testbool = true;
//            //Act
//            testobj1.IsControlWork = testbool;
//            //Assert
//            Assert.Equal(testobj1.IsControlWork, testbool);

//        }
//    }
//}
