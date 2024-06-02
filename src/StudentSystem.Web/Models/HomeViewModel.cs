namespace StudentSystem.Web.Models
{
    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Students.DTOs.ViewModels;

    public class HomeViewModel
    {
        public StudentDashboardViewModel StudentDashboard { get; set; }

        public bool IsActiveStudent { get; set; }

        public IEnumerable<LatestCourseViewModel> CoursesSlider { get; set; }

        public bool HasCoureses => !this.CoursesSlider.IsNullOrEmpty();
    }
}
