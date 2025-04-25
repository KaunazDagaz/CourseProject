using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface IAnswerService
    {
        Task<TemplateSubmissionViewModel> GetTemplateForSubmissionAsync(Guid templateId);
        Task SaveAnswersAsync(List<AnswerSubmissionViewModel> answers, string userId);
    }
}
