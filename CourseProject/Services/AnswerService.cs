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

        public async Task LoadPreviousAnswersAsync(List<FormSubmissionViewModel> forms, string userId)
        {
            var formIds = forms.Select(f => f.FormId).ToList();
            var answers = await dbContext.Answers.Where(a => formIds.Contains(a.FormId) && a.AuthorId == userId)
                .ToListAsync();
            var answersFormId = answers.ToDictionary(a => a.FormId, a => a);
            var answerIds = answers.Select(a => a.Id).ToList();
            var answerOptions = await dbContext.AnswerOptions.Where(ao => answerIds.Contains(ao.AnswerId))
                .GroupBy(ao => ao.AnswerId)
                .ToDictionaryAsync(g => g.Key, g => g.Select(ao => ao.OptionId).ToList());
            foreach (var form in forms)
            {
                if (answersFormId.TryGetValue(form.FormId, out var answer))
                {
                    form.Answer = new AnswerSubmissionViewModel
                    {
                        FormId = answer.FormId,
                        QuestionId = form.Question.Id,
                        QuestionType = form.Question.Type,
                        TextAnswer = answer.TextAnswer
                    };
                    if (form.Question.Type == QuestionType.Checkbox && answerOptions.TryGetValue(answer.Id, out var options))
                        form.Answer.SelectedOptions = options;
                }
            }
        }

        public async Task SaveAnswersAsync(List<AnswerSubmissionViewModel> answers, string userId)
        {
            await AddAnswerAsync(answers, userId);
            await UpdateTemplateStatsAsync(answers.First().FormId);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasAnsweredAsync(Guid templateId, string userId)
        {
            var formIds = await dbContext.Forms.Where(f => f.TemplateId == templateId)
                .Select(f => f.Id)
                .ToListAsync();
            return await dbContext.Answers.AnyAsync(a => formIds.Contains(a.FormId) && a.AuthorId == userId);
        }

        public async Task UpdateAnswersAsync(List<AnswerSubmissionViewModel> answers, string userId)
        {
            var templateId = await dbContext.Forms.Where(f => f.Id == answers.First().FormId)
                .Select(f => f.TemplateId)
                .FirstOrDefaultAsync();
            var formIds = await dbContext.Forms.Where(f => f.TemplateId == templateId)
                .Select(f => f.Id)
                .ToListAsync();
            var existingAnswers = await dbContext.Answers.Where(a => formIds.Contains(a.FormId) && a.AuthorId == userId)
                .ToListAsync();
            foreach (var answer in existingAnswers)
            {
                var options = await dbContext.AnswerOptions.Where(ao => ao.AnswerId == answer.Id)
                    .ToListAsync();
                dbContext.AnswerOptions.RemoveRange(options);
            }
            dbContext.Answers.RemoveRange(existingAnswers);
            await AddAnswerAsync(answers, userId);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<RespondentViewModel>> GetTemplateRespondentsAsync(Guid templateId)
        {
            var formIds = await dbContext.Forms.Where(f => f.TemplateId == templateId)
                .Select(f => f.Id)
                .ToListAsync();
            return await dbContext.Answers.Where(a => formIds.Contains(a.FormId))
                .GroupBy(a => a.AuthorId)
                .Select(g => new
                {
                    Id = g.Key,
                    g.First().SubmittedAt
                })
                .Join(
                    dbContext.Users,
                    r => r.Id,
                    u => u.Id,
                    (r, u) => new RespondentViewModel
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email!,
                        SubmittedAt = r.SubmittedAt
                    })
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();
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
                    result.Add(formViewModel);
            }
            return result;
        }

        private async Task<FormSubmissionViewModel?> BuildFormSubmissionViewModelAsync(Form form)
        {
            var question = await dbContext.Questions.FirstOrDefaultAsync(q => q.FormId == form.Id);
            if (question == null)
                return null;
            var formViewModel = mapper.Map<FormSubmissionViewModel>(form);
            formViewModel.Question = mapper.Map<QuestionViewModel>(question);
            var answerViewModel = mapper.Map<AnswerSubmissionViewModel>(question);
            answerViewModel.FormId = form.Id;
            formViewModel.Answer = answerViewModel;
            if (question.Type == QuestionType.Checkbox)
                formViewModel.Options = await GetQuestionOptionsAsync(question.Id);
            return formViewModel;
        }

        private async Task<List<QuestionOptionViewModel>> GetQuestionOptionsAsync(Guid questionId)
        {
            var options = await dbContext.QuestionOptions.Where(o => o.QuestionId == questionId)
                .OrderBy(o => o.Position)
                .ToListAsync();
            return mapper.Map<List<QuestionOptionViewModel>>(options);
        }

        private async Task AddAnswerAsync(List<AnswerSubmissionViewModel> answers, string userId)
        {
            var now = DateTime.UtcNow;
            foreach (var answerModel in answers)
            {
                var answer = await CreateAnswerAsync(answerModel, userId, now);
                await dbContext.Answers.AddAsync(answer);
                if (answerModel.QuestionType == QuestionType.Checkbox && answerModel.SelectedOptions != null &&
                    answerModel.SelectedOptions.Any())
                    await CreateAnswerOptionsAsync(answer.Id, answerModel.SelectedOptions!);
            }
        }

        private async Task<Answer> CreateAnswerAsync(AnswerSubmissionViewModel model, string userId, DateTime submittedAt)
        {
            return await Task.Run(() =>
            {
                var answer = mapper.Map<Answer>(model);
                answer.Id = Guid.NewGuid();
                answer.AuthorId = userId;
                answer.SubmittedAt = submittedAt;
                return answer;
            });
        }

        private async Task CreateAnswerOptionsAsync(Guid answerId, List<Guid> optionIds)
        {
            foreach (var optionId in optionIds)
            {
                var answerOption = new AnswerOption
                {
                    Id = Guid.NewGuid(),
                    AnswerId = answerId,
                    OptionId = optionId
                };
                await dbContext.AnswerOptions.AddAsync(answerOption);
            }
        }

        private async Task UpdateTemplateStatsAsync(Guid formId)
        {
            var templateId = await dbContext.Forms.Where(f => f.Id == formId)
                .Select(f => f.TemplateId)
                .FirstOrDefaultAsync();
            var stats = await dbContext.TemplateStats.FirstOrDefaultAsync(ts => ts.TemplateId == templateId);
            if (stats == null)
                await CreateNewTemplateStats(templateId);
            else
            {
                stats.AnswersCount++;
                dbContext.TemplateStats.Update(stats);
            }
        }

        private async Task CreateNewTemplateStats(Guid templateId)
        {
            var stats = new TemplateStats
            {
                Id = Guid.NewGuid(),
                TemplateId = templateId,
                AnswersCount = 1
            };
            await dbContext.TemplateStats.AddAsync(stats);
        }
    }
}