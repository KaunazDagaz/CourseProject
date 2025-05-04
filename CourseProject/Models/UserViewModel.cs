namespace CourseProject.Models
{
    public class UserViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class RespondentViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

}
