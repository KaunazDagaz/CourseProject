using CourseProject.Entities;
using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface ITemplateService
    {
        List<TemplateGalleryViewModel> GetLatestsTemplates(int count);
        List<TemplateTableViewModel> GetPopularTemplates(int count);
        Task<Template> CreateTemplateAsync(TemplateCreateViewModel templateViewModel, string userId);
        Task SaveTemplateAsync(Template template);
        Task<TemplateViewModel> GetTemplateAsync(Guid id);
        Task<List<TemplateTableViewModel>> GetUserCreatedTemplatesAsync(string userId);
        Task<List<TemplateTableViewModel>> GetUserFilledTemplatesAsync(string userId);
    }
}
