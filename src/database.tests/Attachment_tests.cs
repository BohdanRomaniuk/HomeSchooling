using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using database.Models;

namespace database.tests
{
   public  class Attachment_tests
    {

        [Fact]
        public void A_test()
        {
            string test_name = "text";
            User test_user = new User();
            test_user.Name = "text1";
            DateTime test_time = DateTime.Today;
            Attachment test_obj = new Attachment(test_name,test_user,test_time);
            test_obj.Id = 2;

            //Act
            string test_name2 = "text3";
            User test_user2 = new User();
            test_user2.Name = "text4";
            DateTime test_time2 = new DateTime(2018,5,7,22,48,11);
            test_obj.Id = 2;
            test_obj.FileName = test_name2;
            test_obj.UploadedBy = test_user2;
            test_obj.UploadDate = test_time2;

            Assert.Equal(test_obj.FileName , test_name2);
            Assert.Equal(test_obj.UploadedBy , test_user2);
            Assert.Equal(test_obj.UploadDate , test_time2);
        }
    }
}
