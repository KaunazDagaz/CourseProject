using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface ISearchService
    {
        Task<IEnumerable<TemplateGalleryViewModel>> SearchTemplatesAsync(
            string query,
            bool includePrivate = false,
            string? userId = null,
            int limit = 50,
            double similarityThreshold = 0.1);
    }
}
