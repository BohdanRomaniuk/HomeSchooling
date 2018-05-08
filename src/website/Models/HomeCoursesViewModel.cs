using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public class HomeCoursesViewModel
    {
        public string Category { get; set; }
        public IEnumerable<HomeViewModel> Courses { get; set; }
        public HomeCoursesViewModel(IEnumerable<HomeViewModel> _courses, string _category)
        {
            Category = _category;
            Courses = _courses;
        }
    }
}
