namespace StudentSystem.Services.Data.Features.StudentCourses.DTOs.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;

    using static StudentSystem.Services.Data.Features.StudentCourses.Constants.StudentCoursesConstants;

    public class EmailInformationBindingModel
    {
        [Required]
        [StringLength(SubjectMaxLength)]
        public string Subject { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        public IEnumerable<IFormFile>? Attachments { get; set; }

        [Display(Name = "Courses")]
        public IEnumerable<Guid> SelectedCourses { get; set; }

        public IEnumerable<CourseSelectionItemViewModel> Courses { get; set; } = new List<CourseSelectionItemViewModel>();
    }
}
