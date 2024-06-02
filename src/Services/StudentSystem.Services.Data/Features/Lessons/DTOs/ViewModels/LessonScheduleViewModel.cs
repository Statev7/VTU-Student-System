namespace StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels
{
    public class LessonScheduleViewModel : LessonMetaDataViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string CourseName { get; set; } = null!;

        public Guid CourseId { get; set; }
    }
}
