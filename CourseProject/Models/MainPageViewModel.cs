using CourseProject.Entities;

namespace CourseProject.Models
{
    public class MainPageViewModel
    {
        public List<TemplateGalleryViewModel>? LatestTemplates { get; set; }
        public List<TemplateTableViewModel>? PopularTemplates { get; set; }
        public List<Tag>? Tags { get; set; }

    }
}
