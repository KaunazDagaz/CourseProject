using CourseProject.Entities;

namespace CourseProject.Services.IServices
{
    public interface IUserValidationService
    {
        Task<bool> IsCurrentUserValidAsync();
        Task<bool> IsCurrentUserAdminAsync();
        Task<bool> IsCurrentUserIncludedAsync(List<string> userIds);
        Task<User?> GetCurrentUserAsync();
    }
}
