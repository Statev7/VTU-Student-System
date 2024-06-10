namespace StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels
{
    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Features.Resources.DTOs.ViewModels;

    public class LessonDetailsViewModel : LessonMetaDataViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Guid CourseId { get; set; }

        public IEnumerable<ResourceViewModel> Resources { get; set; }

        public bool HasResources => !this.Resources.IsNullOrEmpty();
    }
}
