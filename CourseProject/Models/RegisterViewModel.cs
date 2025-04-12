using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Error.NameRequired")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Error.EmailRequired")]
        [EmailAddress(ErrorMessage = "Error.EmailInvalid")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Error.PasswordRequired")]
        [MinLength(1, ErrorMessage = "Error.PasswordEmpty")]
        public required string Password { get; set; }
    }
}
