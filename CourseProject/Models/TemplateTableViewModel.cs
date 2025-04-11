namespace CourseProject.Models
{
    public class TemplateTableViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
