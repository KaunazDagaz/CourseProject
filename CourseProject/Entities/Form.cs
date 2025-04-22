using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class Form
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid TemplateId { get; set; }
        [Required]
        public int Position { get; set; }
    }
}
