using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using database.Models;

namespace website.Models
{
    public class HomeViewModel
    {
        public bool? ShowRequestCourse { get; set; }
        public Course CurrentCourse { get; set; }
        public HomeViewModel(Course _course, bool? _show)
        {
            CurrentCourse = _course;
            ShowRequestCourse = _show;
        }
    }
}
