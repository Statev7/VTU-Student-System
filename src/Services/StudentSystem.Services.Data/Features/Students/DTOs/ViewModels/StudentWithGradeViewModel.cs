namespace StudentSystem.Services.Data.Features.Students.DTOs.ViewModels
{
    public class StudentWithGradeViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public Guid? ExamId { get; set; }

        public int? Grade { get; set; }

        public string GradeAsText => this.Grade.HasValue
            ? this.Grade.Value.ToString()
            : "none";
    }
}
