using CourseProject.Entities;

namespace CourseProject.Models
{
    public class AnswerSubmissionViewModel
    {
        public Guid FormId { get; set; }
        public Guid QuestionId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string? TextAnswer { get; set; }
        public List<Guid>? SelectedOptions { get; set; }
    }

    public class TemplateSubmissionViewModel
    {
        public Guid TemplateId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<FormSubmissionViewModel> Forms { get; set; } = new();
    }

    public class FormSubmissionViewModel
    {
        public Guid FormId { get; set; }
        public int Position { get; set; }
        public QuestionViewModel Question { get; set; } = null!;
        public List<QuestionOptionViewModel>? Options { get; set; }
        public AnswerSubmissionViewModel Answer { get; set; } = new();
    }

    public class QuestionOptionViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}
