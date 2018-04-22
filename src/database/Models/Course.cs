using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace database.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public User Teacher { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public List<Lesson> CourseLessons { get; set; }

        public Course()
        {
        }

        public Course(string _name, string _description, User _teacher, DateTime _startDate, DateTime _endDate)
        {
            Name = _name;
            Description = _description;
            Teacher = _teacher;
            StartDate = _startDate;
            EndDate = _endDate;
        }
    }
}
