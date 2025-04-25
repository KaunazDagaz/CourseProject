namespace CourseProject.Models
{
    public class FormWithQuestionsViewModel
    {
        public QuestionCreateViewModel Question { get; set; } = new QuestionCreateViewModel() { Title = string.Empty };
        public int Position { get; set; }
        public bool ShowInTable { get; set; }
    }
}