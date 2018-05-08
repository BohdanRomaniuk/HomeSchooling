using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using database.Models;

namespace website.Models
{
    public class CourseStudentsModel
    {
        [Required]
        public IQueryable<User> Students { get; set; }
        [Required]
        public string CourseName { get; set; }
        public CourseStudentsModel(IQueryable<User> _students, string _courseName)
        {
            Students = _students;
            CourseName = _courseName;
        }
    }
}
