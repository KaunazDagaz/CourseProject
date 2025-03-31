using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class TemplateTag
    {
        [Required]
        public Guid TemplateId { get; set; }
        [Required]
        public Guid TagId { get; set; }
    }
}