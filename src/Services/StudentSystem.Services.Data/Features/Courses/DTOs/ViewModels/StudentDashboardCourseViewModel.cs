namespace StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels
{
    public class StudentDashboardCourseViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }

        public int? ExamGrade { get; set; }
    }
}
