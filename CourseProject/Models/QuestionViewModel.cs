using CourseProject.Entities;

namespace CourseProject.Models
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public QuestionType Type { get; set; }
        public bool ShowInTable { get; set; }
        public int? MaxLength { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }
}
