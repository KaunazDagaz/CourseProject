using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
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
}
