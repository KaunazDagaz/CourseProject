using CourseProject.Entities;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models
{
    public class QuestionCreateViewModel
    {
        [Required(ErrorMessage = "Question title is required")]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Question type is required")]
        public QuestionType Type { get; set; }
        public bool ShowInTable { get; set; }
    }
}
