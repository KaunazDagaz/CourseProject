using CourseProject.Entities;
using CourseProject.Models;

namespace CourseProject.Services.IServices
{
    public interface IFormService
    {
        Task<List<Form>> GetAllFormsAsync(Guid templateId);
        Form CreateForm(Guid templateId, FormWithQuestionsViewModel formViewModel);
        Task SaveFormAsync(Form form);
    }
}
