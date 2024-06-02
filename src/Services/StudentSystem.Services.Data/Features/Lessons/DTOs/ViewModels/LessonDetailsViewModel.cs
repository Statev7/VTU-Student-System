namespace StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels
{
    public class LessonDetailsViewModel : LessonMetaDataViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
