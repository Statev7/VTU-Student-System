namespace StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels
{
    using StudentSystem.Services.Data.Features.Courses.Enums;

    public class CoursesRequestDataModel
    {
        public int CurrentPage { get; set; }

        public string SearchTerm { get; set; } = null!;

        public CoursesOrderOptions OrderBy { get; set; }
    }
}
