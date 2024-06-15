namespace StudentSystem.Services.Data.Features.StudentCourses.DTOs.RequestDataModels
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;

    public class StudentsInCourseRequestData
    {
        public string SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public Guid CourseId { get; set; }

        public IEnumerable<CourseSelectionItemViewModel> Courses { get; set; }
    }
}
