using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid FormId { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        public int? MaxLength { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }

    public class QuestionOption
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid QuestionId { get; set; }
        [Required]
        public required string Text { get; set; }
        [Required]
        public int Position { get; set; }
    }

    public enum QuestionType
    {
        SingleLine,
        MultiLine,
        Integer,
        Checkbox
    }

}
