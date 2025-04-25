using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class AnswerOption
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid AnswerId { get; set; }
        [Required]
        public Guid OptionId { get; set; }
    }
}
