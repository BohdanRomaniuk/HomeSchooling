using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue("false")]
        public bool IsControlWork { get; set; }

        public List<Post> Posts { get; set; }
        

        public Lesson()
        {
            Posts = new List<Post>();
        }

        public Lesson(string _name, DateTime _date, bool _isCOntrolWork=false)
        {
            Posts = new List<Post>();
            Name = _name;
            Date = _date;
            IsControlWork = _isCOntrolWork;
        }
    }
}
