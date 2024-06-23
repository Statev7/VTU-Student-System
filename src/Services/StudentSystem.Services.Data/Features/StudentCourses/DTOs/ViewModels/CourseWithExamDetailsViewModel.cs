namespace StudentSystem.Services.Data.Features.StudentCourses.DTOs.ViewModels
{
    using static StudentSystem.Services.Data.Features.Courses.Constants.CourseConstants;

    public class CourseWithExamDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Credits { get; set; }

        public int? Grade { get; set; }

        public string GradeComment { get; set; } = null!;

        public string FormatedStartDate => this.StartDate.ToString(DateFormat);

        public string FormatedEndDate => this.EndDate.ToString(DateFormat);

        public string FormatedGrade => this.Grade.HasValue
            ? this.Grade.Value.ToString("F2")
            : "-";

        public string FormatedCredits => this.Grade.HasValue 
            ? this.Credits.ToString()
            : "-";
    }
}
