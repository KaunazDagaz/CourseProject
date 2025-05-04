using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public required string AuthorId { get; set; }
        [Required]
        public Guid FormId { get; set; }
        [Required]
        public DateTime SubmittedAt { get; set; }
        public string? TextAnswer { get; set; }
    }

    public class AnswerOption
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid AnswerId { get; set; }
        [Required]
        public Guid OptionId { get; set; }
    }
}
