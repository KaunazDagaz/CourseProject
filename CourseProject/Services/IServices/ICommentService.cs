using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface ICommentService
    {
        Task<List<CommentViewModel>> GetCommentsAsync(Guid templateId);
        Task<(bool success, Guid commentId, DateTime timestamp)> AddCommentAsync(CommentCreateViewModel model, string userId, string authorName);
    }
}
