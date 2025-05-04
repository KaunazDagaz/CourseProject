using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface IAnswerService
    {
        Task<TemplateSubmissionViewModel> GetTemplateForSubmissionAsync(Guid templateId);
        Task LoadPreviousAnswersAsync(List<FormSubmissionViewModel> forms, string userId);
        Task SaveAnswersAsync(List<AnswerSubmissionViewModel> answers, string userId);
        Task<bool> HasAnsweredAsync(Guid templateId, string userId);
        Task UpdateAnswersAsync(List<AnswerSubmissionViewModel> answers, string userId);
        Task<List<RespondentViewModel>> GetTemplateRespondentsAsync(Guid templateId);
    }
}
