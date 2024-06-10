namespace StudentSystem.Services.Data.Features.Resources.DTOs.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels;

    using static StudentSystem.Data.Common.Constants.Resources;

    public class ResourceBindingModel
    {
        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; }

        public IFormFile? File { get; set; }

        [Display(Name = "Upload New Resource")]
        public bool UploadNewResource { get; set; }

        [Display(Name = "Lesson")]
        public Guid LessonId { get; set; }

        public IEnumerable<LessonSelectionItemViewModel> Lessons { get; set; } = new List<LessonSelectionItemViewModel>();
    }
}
