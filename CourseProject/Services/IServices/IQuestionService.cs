using CourseProject.Entities;
using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface IQuestionService
    {
        Task<QuestionViewModel> GetQuestionAsync(Guid formId);
        Task<Question> CreateQuestion(QuestionCreateViewModel questionViewModel, Guid formId);
        Task SaveQuestionAsync(Question question, List<string>? checkboxOptions);
        Task UpdateQuestionAsync(QuestionViewModel question);
    }
}
