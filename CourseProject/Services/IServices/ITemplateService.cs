using CourseProject.Entities;
using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface ITemplateService
    {
        List<TemplateGalleryViewModel> GetLatestsTemplates(int count);
        List<TemplateTableViewModel> GetPopularTemplates(int count);
    }
}
