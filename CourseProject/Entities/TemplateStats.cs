using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class TemplateStats
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public int AnswersCount { get; set; }

    }
}
