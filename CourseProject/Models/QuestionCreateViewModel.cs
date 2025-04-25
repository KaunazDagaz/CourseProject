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
        public int? MaxLength { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public List<string> CheckboxOptions { get; set; } = new List<string>();
    }
}
