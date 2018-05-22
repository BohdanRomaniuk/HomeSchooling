using System;
using System.Collections.Generic;
using System.Text;
using website.Models;
using Xunit;

namespace website.tests
{
   public class LoginModelTests
    {
        [Fact]
        public void Test()
        {
            LoginModel temp = new LoginModel();
            string temp1 = "text1";
            string temp2 = "text2";
            temp.UserName = temp1;
            temp.Password = temp2;
            Assert.Equal(temp1, temp.UserName);
            Assert.Equal(temp2, temp.Password);
        }
    }
}
