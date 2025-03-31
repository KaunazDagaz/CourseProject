using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}
