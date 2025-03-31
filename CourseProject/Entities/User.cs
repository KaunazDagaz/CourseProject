using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public bool IsBlocked { get; set; } = false;
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
