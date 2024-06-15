namespace StudentSystem.Services.Data.Features.Exam.DTOs.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants.Exam;

    public class UpdateExamBindingModel
    {
        [Range(MinGradeValue, MaxGradeValue)]
        public int Grade { get; set; }

        [MaxLength(CommentMaxLength)]
        public string Comment { get; set; } = null!;
    }
}
