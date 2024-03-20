﻿namespace StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Common.Attributes;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.ViewModels;

    using static StudentSystem.Data.Common.Constants.Course;

    public class CourseFormBidningModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public ComplexityLevel Complexity { get; set; }

        [Range(0, int.MaxValue)]
        public int Credits { get; set; }

        [Display(Name = "Start Date")]
        [DateLessThanAttribute(nameof(EndDate))]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Now;

        [Display(Name = "Teacher")]
        public Guid TeacherId { get; set; }

        public IEnumerable<TeacherSelectionItemViewModel> Teachers { get; set; } = new List<TeacherSelectionItemViewModel>();
    }
}