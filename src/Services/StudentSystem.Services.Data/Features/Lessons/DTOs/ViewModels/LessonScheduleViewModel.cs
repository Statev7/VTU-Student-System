namespace StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels
{
    public class LessonScheduleViewModel
    {
        public string Name { get; set; } = null!;

        public string CourseName { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
