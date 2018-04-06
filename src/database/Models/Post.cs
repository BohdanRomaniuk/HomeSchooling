using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace database.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
    }
}
