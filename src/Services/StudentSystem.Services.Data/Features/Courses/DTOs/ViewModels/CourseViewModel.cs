namespace StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels
{
    using StudentSystem.Data.Models.Courses;

    public class CourseViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string TeaserDescription { get; set; } = null!;

        public string StartDate { get; set; } = null!;

        public int Duration { get; set; }

        public ComplexityLevel Complexity { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string DurationText => this.Duration > 1 ? $"{this.Duration} weeks" : $"{this.Duration} week";
    }
}
