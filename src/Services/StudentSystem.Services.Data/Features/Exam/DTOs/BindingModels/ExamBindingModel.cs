namespace StudentSystem.Services.Data.Features.Exam.DTOs.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants.Exam;

    public class ExamBindingModel
    {
        [Range(MinGradeValue, MaxGradeValue)]
        public int? Grade { get; set; }

        [MaxLength(CommentMaxLength)]
        public string Comment { get; set; } = null!;

        public Guid CourseId { get; set; }

        public Guid StudentId { get; set; }
    }
}
