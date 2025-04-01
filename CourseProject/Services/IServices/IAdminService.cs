using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetUsers();
        Task<bool> IsCurrentUserValidAsync();
        Task<bool> IsCurrentUserIncludedAsync(List<string> userIds);
    }
}
