using CourseProject.Entities;
using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface IQuestionService
    {
        Task<QuestionViewModel> GetQuestionAsync(Guid formId);
        Question CreateQuestion(QuestionCreateViewModel questionViewModel, Guid formId);
        Task SaveQuestionAsync(Question question);
        Task UpdateQuestionAsync(QuestionViewModel question);
    }
}
