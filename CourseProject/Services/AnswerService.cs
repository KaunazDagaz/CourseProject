using AutoMapper;
using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public AnswerService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<TemplateSubmissionViewModel> GetTemplateForSubmissionAsync(Guid templateId)
        {
            var template = await dbContext.Templates.FirstOrDefaultAsync(t => t.Id == templateId);
            if (template == null)
                return null!;
            var result = mapper.Map<TemplateSubmissionViewModel>(template);
            result.Forms = await BuildFormSubmissionViewModelsAsync(templateId);
            return result;
        }

        public async Task SaveAnswersAsync(List<AnswerSubmissionViewModel> answers, string userId)
        {
            await EnsureFormIdsAsync(answers);
            AddAnswerAsync(answers, userId);
            await UpdateTemplateStatsAsync(answers.First().FormId);
            await dbContext.SaveChangesAsync();
        }

        private async Task EnsureFormIdsAsync(List<AnswerSubmissionViewModel> answers)
        {
            var missingFormIds = answers.Where(a => a.FormId == Guid.Empty).ToList();
            if (!missingFormIds.Any())
                return;
            var questionIds = missingFormIds.Select(a => a.QuestionId).ToList();
            var questionFormMap = await dbContext.Questions.Where(q => questionIds.Contains(q.Id))
                .ToDictionaryAsync(q => q.Id, q => q.FormId);
            foreach (var answer in missingFormIds)
            {
                if (questionFormMap.TryGetValue(answer.QuestionId, out var formId))
                {
                    answer.FormId = formId;
                }
            }
        }

        private async Task<List<FormSubmissionViewModel>> BuildFormSubmissionViewModelsAsync(Guid templateId)
        {
            var result = new List<FormSubmissionViewModel>();
            var forms = await dbContext.Forms.Where(f => f.TemplateId == templateId)
                .OrderBy(f => f.Position)
                .ToListAsync();
            foreach (var form in forms)
            {
                var formViewModel = await BuildFormSubmissionViewModelAsync(form);
                if (formViewModel != null)
                {
                    result.Add(formViewModel);
                }
            }
            return result;
        }

        private async Task<FormSubmissionViewModel?> BuildFormSubmissionViewModelAsync(Form form)
        {
            var question = await dbContext.Questions.FirstOrDefaultAsync(q => q.FormId == form.Id);
            if (question == null) return null;
            var formViewModel = mapper.Map<FormSubmissionViewModel>(form);
            formViewModel.Question = mapper.Map<QuestionViewModel>(question);
            formViewModel.Answer = mapper.Map<AnswerSubmissionViewModel>(question);
            if (question.Type == QuestionType.Checkbox)
            {
                formViewModel.Options = await GetQuestionOptionsAsync(question.Id);
            }
            return formViewModel;
        }

        private async Task<List<QuestionOptionViewModel>> GetQuestionOptionsAsync(Guid questionId)
        {
            var options = await dbContext.QuestionOptions.Where(o => o.QuestionId == questionId)
                .OrderBy(o => o.Position)
                .ToListAsync();
            return mapper.Map<List<QuestionOptionViewModel>>(options);
        }

        private void AddAnswerAsync(List<AnswerSubmissionViewModel> answers, string userId)
        {
            var now = DateTime.UtcNow;
            foreach (var answerModel in answers)
            {
                var answer = CreateAnswer(answerModel, userId, now);
                dbContext.Answers.Add(answer);
                if (answerModel.QuestionType == QuestionType.Checkbox && answerModel.SelectedOptions != null &&
                    answerModel.SelectedOptions.Any())
                {
                    CreateAnswerOptions(answer.Id, answerModel.SelectedOptions!);
                }
            }
        }

        private Answer CreateAnswer(AnswerSubmissionViewModel model, string userId, DateTime submittedAt)
        {
            var answer = mapper.Map<Answer>(model);
            answer.Id = Guid.NewGuid();
            answer.AuthorId = userId;
            answer.SubmittedAt = submittedAt;
            return answer;
        }

        private void CreateAnswerOptions(Guid answerId, List<Guid> optionIds)
        {
            foreach (var optionId in optionIds)
            {
                var answerOption = new AnswerOption
                {
                    Id = Guid.NewGuid(),
                    AnswerId = answerId,
                    OptionId = optionId
                };
                dbContext.AnswerOptions.Add(answerOption);
            }
        }

        private async Task UpdateTemplateStatsAsync(Guid formId)
        {
            var templateId = await GetTemplateIdFromFormAsync(formId);
            var stats = await dbContext.TemplateStats.FirstOrDefaultAsync(ts => ts.TemplateId == templateId);
            if (stats == null)
            {
                CreateNewTemplateStats(templateId);
            }
            else
            {
                stats.AnswersCount++;
                dbContext.TemplateStats.Update(stats);
            }
        }

        private async Task<Guid> GetTemplateIdFromFormAsync(Guid formId)
        {
            return await dbContext.Forms.Where(f => f.Id == formId)
                .Select(f => f.TemplateId)
                .FirstOrDefaultAsync();
        }

        private void CreateNewTemplateStats(Guid templateId)
        {
            var stats = new TemplateStats
            {
                Id = Guid.NewGuid(),
                TemplateId = templateId,
                AnswersCount = 1
            };
            dbContext.TemplateStats.Add(stats);
        }
    }
}