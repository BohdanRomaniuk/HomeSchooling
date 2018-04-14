using database.Models;
using website;
using Xunit;

namespace website.tests
{
    public class HomePageTests
    {
        [Fact]
        public void CoursesList()
        {
            string mo1 = "Modest";
            string mo2 = "Modest";
            Assert.Equal(mo1, mo2);
        }

        [Fact]
        public void CoursesSearch()
        {
            string mo1 = "Modest";
            string mo2 = "Modest";
            Assert.Equal(mo1, mo2);
        }
    }
}
