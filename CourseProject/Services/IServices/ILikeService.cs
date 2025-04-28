namespace CourseProject.Services.IServices
{
    public interface ILikeService
    {
        Task<(bool success, bool userLiked)> ToggleLikeAsync(Guid templateId, string userId);
        Task<bool> HasUserLikedTemplateAsync(Guid templateId, string userId);
        Task<int> GetLikesCountAsync(Guid templateId);
    }
}
