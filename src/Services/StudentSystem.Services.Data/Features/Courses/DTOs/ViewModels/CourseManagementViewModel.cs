namespace StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels
{
    public class CourseManagementViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string StartDate { get; set; } = null!;
    }
}
