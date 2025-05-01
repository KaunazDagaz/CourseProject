namespace CourseProject.Models
{
    public class ProfileViewModel
    {
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public List<TemplateTableViewModel> CreatedTemplates { get; set; } = new List<TemplateTableViewModel>();
        public List<TemplateTableViewModel> FilledTemplates { get; set; } = new List<TemplateTableViewModel>();
    }
}
