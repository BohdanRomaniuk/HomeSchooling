using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace database.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public User PostedBy { get; set; }
        public string Text { get; set; }
        [Required]
        public DateTime PostedDate { get; set; }

        List<Attachment> PostAtachments { get; set; }
    }
}
