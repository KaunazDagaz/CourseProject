using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models
{
    public class TemplateCreateViewModel
    {
        [Required(ErrorMessage = "Error.TitleReuired")]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Error.TopicRequired")]
        public required string Topic { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public bool IsPublic { get; set; } = true;
    }
}
