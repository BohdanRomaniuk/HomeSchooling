using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace database.Models
{
    public class User: IdentityUser
    {
        [Required]
        public string Name { get; set; }
    }
}
