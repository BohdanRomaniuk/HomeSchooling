using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace database.Models
{
    public class CoursesListener
    {
        [Key]
        public int Id { get; set; }
        public User Student { get; set; }
        public Course RequestedCourse {get;set;}
        [Required]
        [DefaultValue("faulse")]
        public bool Accepted { get; set; }
        public CoursesListener(int _id, User _student, Course _requestedCourse)
        {
            Id = _id;
            Student = _student;
        }
        public CoursesListener()
        {
        }
    }
}
