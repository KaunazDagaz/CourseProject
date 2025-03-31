namespace CourseProject.Entities
{
    public class Like
    {
        public Guid Id { get; set; }
        public required string AuthorId { get; set; }
        public Guid TemplateId { get; set; }
    }
}
