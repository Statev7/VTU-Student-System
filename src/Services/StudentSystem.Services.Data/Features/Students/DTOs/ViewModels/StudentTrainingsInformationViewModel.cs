namespace StudentSystem.Services.Data.Features.Students.DTOs.ViewModels
{
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;

    public class StudentTrainingsInformationViewModel
    {
        public IEnumerable<StudentDashboardCourseViewModel> Courses { get; set; }

        public double? AvgGrade { get; set; }

        public int TotalCredits { get; set; }
    }
}
