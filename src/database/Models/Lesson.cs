using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace database.Models
{
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public bool IsControlWork { get; set; }
        
        public string LessonDescription { get; set; }
        public string HomeWorkDescription { get; set; }

        public Lesson()
        {
            IsControlWork = false;
        }

        public Lesson(string _name, DateTime _date, bool _isCOntrolWork=false)
        {
            Name = _name;
            Date = _date;
            IsControlWork = _isCOntrolWork;
        }
    }
}
