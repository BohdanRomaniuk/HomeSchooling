﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace database.Models
{
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }
        public User UploadedBy { get; set; }
        [Required]
        public string FileName { get; set; }
        public Attachment()
        { }
        public Attachment ( string name,User user,DateTime upload_date)
        {
            FileName = name;
            UploadedBy = user;
            UploadDate = upload_date;
        }
    }
}
