using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace database.Models
{
    public class Mark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Post")]
        public Post PostId { get; set; }

        public User Student { get; set; }
        public User Teacher { get; set; }
        [Required]
        public int MarkValue { get; set; }
        [Required]
        public DateTime MarkDate { get; set; }
        public Lesson MarkedLesson { get; set; }

        public Mark()
        {
        }
        public Mark(User _student, User _teacher, int _markValue,  DateTime _markedDate, Lesson _markedLesson)
        {
            Student = _student;
            Teacher = _teacher;
            MarkValue = _markValue;
            MarkDate = _markedDate;
            MarkedLesson = _markedLesson;
        }
    }
}
