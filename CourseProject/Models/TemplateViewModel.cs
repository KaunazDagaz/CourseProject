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
    }
}
