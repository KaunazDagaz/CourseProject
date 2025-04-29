using CourseProject.Entities;

namespace CourseProject.Services.IServices
{
    public interface ITagService
    {
        Task<List<Tag>> SearchTagsAsync(string query, int limit = 5);
        Task<List<Tag>> GetPopularTagsAsync(int limit = 20);
        Task AddTagsToTemplateAsync(Guid templateId, IEnumerable<string> tagNames);
    }
}
