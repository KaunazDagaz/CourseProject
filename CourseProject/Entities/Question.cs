﻿using System.ComponentModel.DataAnnotations;

namespace CourseProject.Entities
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid TemplateId { get; set; }
        [Required]
        public required string AuthorId { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        [Required]
        public int Position { get; set; }
        [Required]
        public bool ShowInTable { get; set; }
    }

    public enum QuestionType
    {
        SingleLine,
        MultiLine,
        Integer,
        Checkbox
    }

}
