using AutoMapper;
using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public QuestionService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<QuestionViewModel> GetQuestionAsync(Guid formId)
        {
            var question = await dbContext.Questions.FirstOrDefaultAsync(q => q.FormId == formId);
            return mapper.Map<QuestionViewModel>(question);
        }

        public async Task<Question> CreateQuestion(QuestionCreateViewModel questionViewModel, Guid formId)
        {
            return await Task.Run(() =>
            {
                var question = mapper.Map<Question>(questionViewModel);
                question.Id = Guid.NewGuid();
                question.FormId = formId;
                return question;
            });
        }

        public async Task SaveQuestionAsync(Question question, List<string>? checkboxOptions)
        {
            await dbContext.Questions.AddAsync(question);
            await dbContext.SaveChangesAsync();
            if (question.Type == QuestionType.Checkbox && checkboxOptions != null && checkboxOptions.Any())
            {
                int position = 0;
                foreach (var optionText in checkboxOptions)
                {
                    var option = new QuestionOption
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = question.Id,
                        Text = optionText,
                        Position = position++
                    };
                    await dbContext.QuestionOptions.AddAsync(option);
                }
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateQuestionAsync(QuestionViewModel questionViewModel)
        {
            var question = await dbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionViewModel.Id);
            question = mapper.Map<Question>(questionViewModel);
            dbContext.Questions.Update(question);
            await dbContext.SaveChangesAsync();
        }
    }
}
