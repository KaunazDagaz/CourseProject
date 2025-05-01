using CourseProject.Entities;
using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface ITemplateService
    {
        Task<List<TemplateGalleryViewModel>> GetLatestsTemplatesAsync(int count);
        Task<List<TemplateTableViewModel>> GetPopularTemplatesAsync(int count);
        Task<Template> CreateTemplateAsync(TemplateCreateViewModel templateViewModel, string userId);
        Task SaveTemplateAsync(Template template);
        Task<TemplateViewModel> GetTemplateAsync(Guid id);
        Task<List<TemplateTableViewModel>> GetUserCreatedTemplatesAsync(string userId);
        Task<List<TemplateTableViewModel>> GetUserFilledTemplatesAsync(string userId);
    }
}
