﻿using System;
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
        
        //public string LessonDescription { get; set; }
        //public string HomeWorkDescription { get; set; }
    }
}
