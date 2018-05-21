using database.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using website.Controllers;
using website.Models;
using Xunit;

namespace website.tests
{
    public class AttachmentControllerTests
    {
        [Fact]
        public void DownloadTest()
        {
            Mock<IHomeSchoolingRepository> mock = new Mock<IHomeSchoolingRepository>();
            Attachment[] attachments = new Attachment[]
            {
                new Attachment("test.txt",null,DateTime.Now) { Id=1 }
            };
            mock.Setup(m => m.Attachments).Returns(attachments.AsQueryable());

            AttachmentController controller = new AttachmentController(mock.Object);
            Task<IActionResult> result = controller.Download(1);
            Assert.True(result.Result is FileStreamResult);
            Assert.Equal("test.txt", (result.Result as FileStreamResult).FileDownloadName);
        }
    }
}
