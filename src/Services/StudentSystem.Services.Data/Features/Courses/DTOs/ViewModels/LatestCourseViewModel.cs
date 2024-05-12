namespace StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels
{
    public class LatestCourseViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Complexity { get; set; } = null!;

        public string StartDate { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
