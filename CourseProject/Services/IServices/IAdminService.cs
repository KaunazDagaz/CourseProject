using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetUsersAsync();
        Task UpdateUserStatusAsync(List<string> userIds, bool status);
        Task RemoveUserAsync(List<string> userIds);
        Task UpdateUserRoleAsync(List<string> userIds, string role);
    }
}
