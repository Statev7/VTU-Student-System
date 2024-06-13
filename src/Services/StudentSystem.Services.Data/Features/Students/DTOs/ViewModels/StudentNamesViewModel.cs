namespace StudentSystem.Services.Data.Features.Students.DTOs.ViewModels
{
    public class StudentNamesViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? Grade { get; set; }

        public string GradeAsText => this.Grade.HasValue
            ? this.Grade.Value.ToString()
            : "none";
    }
}
