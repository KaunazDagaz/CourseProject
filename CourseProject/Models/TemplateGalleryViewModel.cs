namespace CourseProject.Models
{
    public class TemplateGalleryViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string AuthorName { get; set; }
        public string? Image { get; set; }
    }
}
