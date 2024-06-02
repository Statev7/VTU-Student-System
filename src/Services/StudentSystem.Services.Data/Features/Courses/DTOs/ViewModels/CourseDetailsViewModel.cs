namespace StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels
{
    using StudentSystem.Data.Models.Courses.Enums;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.ViewModels;

    public class CourseDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string StartDate { get; set; } = null!;

        public int Duration { get; set; }

        public int Credits { get; set; }

        public ComplexityLevel Complexity { get; set; }

        public string ImageFolder { get; set; } = null!;

        public bool IsUserAlreadyInCourse { get; set; }

        public TeacherViewModel Teacher { get; set; }

        public IEnumerable<LessonPanelViewModel> Lessons { get; set; }
    }
}
