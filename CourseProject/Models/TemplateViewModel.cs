using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models
{
    public class TemplateViewModel
    {
        public Guid Id { get; set; }
        public required string AuthorId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Topic { get; set; }
        public string? Image { get; set; }
        public int LikesCount { get; set; }
    }

    public class TemplateGalleryViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string AuthorName { get; set; }
        public string? Image { get; set; }
    }

    public class TemplateTableViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TemplateCreateViewModel
    {
        [Required(ErrorMessage = "Error.TitleReuired")]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Error.TopicRequired")]
        public required string Topic { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public bool IsPublic { get; set; } = true;
        public string? Tags { get; set; }
    }
}
