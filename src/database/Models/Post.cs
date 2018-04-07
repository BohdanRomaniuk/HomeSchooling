using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue("")]
        public string Text { get; set; }
        [Required]
        public string PostType { get; set; }
        public User PostedBy { get; set; }
        [Required]
        public DateTime PostedDate { get; set; }

        public List<Attachment> PostAtachments { get; set; }

        public Post()
        {
            Text = "";
        }

        public Post(string _text, string _postType, User _postedBy, DateTime _postedData)
        {
            Text = _text;
            PostType = _postType;
            PostedBy = _postedBy;
            PostedDate = _postedData;
        }
    }
}
