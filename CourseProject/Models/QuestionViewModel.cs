using CourseProject.Entities;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public QuestionType Type { get; set; }
        public int? MaxLength { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }

    public class QuestionCreateViewModel
    {
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        public int? MaxLength { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public List<string> CheckboxOptions { get; set; } = new List<string>();
    }
}
