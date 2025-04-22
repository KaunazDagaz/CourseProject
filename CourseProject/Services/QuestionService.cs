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

        public Question CreateQuestion(QuestionCreateViewModel questionViewModel, Guid formId)
        {
            var question = mapper.Map<Question>(questionViewModel);
            question.Id = Guid.NewGuid();
            question.FormId = formId;
            return question;
        }

        public async Task SaveQuestionAsync(Question question)
        {
            dbContext.Questions.Add(question);
            await dbContext.SaveChangesAsync();
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
