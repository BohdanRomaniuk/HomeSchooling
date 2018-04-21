using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace database.Models
{
    public class User: IdentityUser
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //public string UserName { get; set; }
        //[Required]
        //public string Password { get; set; }
        //[Required]
        //public string UserRole { get; set; }
    }
}
