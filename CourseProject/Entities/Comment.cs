using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid TemplateId { get; set; }
        [Required]
        public required string AuthorId { get; set; }
        [Required]
        public required string AuthorName { get; set; }
        [Required]
        public required string Content { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
