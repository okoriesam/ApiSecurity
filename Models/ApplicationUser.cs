using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Api4.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
    }
}
