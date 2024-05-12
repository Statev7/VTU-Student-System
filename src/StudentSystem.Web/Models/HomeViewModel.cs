namespace StudentSystem.Web.Models
{
    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;

    public class HomeViewModel
    {
        public IEnumerable<LatestCourseViewModel> LatestCourses { get; set; }

        public bool HasCoureses => !this.LatestCourses.IsNullOrEmpty();
    }
}
