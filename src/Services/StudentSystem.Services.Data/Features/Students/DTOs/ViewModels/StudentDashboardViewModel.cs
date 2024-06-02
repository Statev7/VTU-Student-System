namespace StudentSystem.Services.Data.Features.Students.DTOs.ViewModels
{
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels;

    public class StudentDashboardViewModel
    {
        public IEnumerable<StudentCourseViewModel> Courses { get; set; }

        public IEnumerable<LessonScheduleViewModel> Schedule { get; set; }
    }
}
