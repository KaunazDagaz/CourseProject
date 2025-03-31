using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class Template
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public required string AuthorId { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public required string Topic { get; set; }
        public string? Image { get; set; }
        [Required]
        public bool IsPublic { get; set; } = true;
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }

}
