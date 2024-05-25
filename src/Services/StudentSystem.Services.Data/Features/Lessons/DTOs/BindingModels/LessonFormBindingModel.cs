namespace StudentSystem.Services.Data.Features.Lessons.DTOs.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using StudentSystem.Common.Attributes;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;

    using static StudentSystem.Data.Common.Constants.Lesson;

    public class LessonFormBindingModel
    {
        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Display(Name = "Start Time")]
        [DateLessThan(nameof(EndTime))]
        public DateTime StartTime { get; set; } = DateTime.Today;

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; } = DateTime.Today;

        [Display(Name = "Course")]
        public Guid CourseId { get; set; }

        public IEnumerable<CourseSelectionItemViewModel> Courses { get; set; } = new List<CourseSelectionItemViewModel>();
    }
}
