using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models
{
    public class TemplateCreateViewModel
    {
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public required string Topic { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public bool IsPublic { get; set; } = true;
    }
}
