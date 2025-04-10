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
        public required string Content { get; set; }
    }
}
