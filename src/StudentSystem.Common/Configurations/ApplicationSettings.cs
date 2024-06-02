namespace StudentSystem.Common
{
    public class ApplicationSettings
    {
        public string SendGridApiKey { get; set; } = null!;

        public string StudentsStatusCronSchedule { get; set; } = null!;

        public string CoursesStatusCronSchedule { get; set; } = null!;
    }
}
