using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format. Format should be example@example")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public required string Password { get; set; }
    }
}
