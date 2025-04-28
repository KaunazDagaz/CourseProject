namespace CourseProject.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public required string AuthorId { get; set; }
        public required string AuthorName { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CommentCreateViewModel
    {
        public Guid TemplateId { get; set; }
        public required string Content { get; set; }
    }

    public class CommentUpdateViewModel
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }
    }
}
