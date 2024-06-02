namespace StudentSystem.Data.Seed.Seeds
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    using StudentSystem.Data.Models.Courses;

    public class LessonSeeder : BaseSeeder<Lesson>
    {
        public LessonSeeder(IServiceScope serviceScope, string jsonData) 
            : base(serviceScope, jsonData)
        {
        }

        public async override Task SeedAsync()
        {
            if (await this.IsAlreadySeedAsync())
            {
                return;
            }

            var lessons = JsonConvert.DeserializeObject<Lesson[]>(this.JsonData);

            var courses = await this.GetCoursesAsync(lessons);

            foreach (var lesson in lessons)
            {
                if (courses.TryGetValue(lesson.CourseId, out DateTime statDate))
                {
                    if (statDate > lesson.StartTime.Date)
                    {
                        var duration = (statDate - lesson.StartTime).Days;

                        lesson.StartTime = lesson.StartTime.AddDays(duration);
                        lesson.EndTime = lesson.EndTime.AddDays(duration);
                    }
                }
            }

            await this.DbSet.AddRangeAsync(lessons);
            await this.DbContext.SaveChangesAsync();
        }

        private async Task<IDictionary<Guid, DateTime>> GetCoursesAsync(Lesson[] lessons)
        {
            var courseIds = lessons
                .Select(l => l.CourseId.ToString())
                .Distinct()
                .ToList();

            var courses = await this.DbContext.Courses
                .Where(c => courseIds.Contains(c.Id.ToString()))
                .ToDictionaryAsync(kp => kp.Id, kp => kp.StartDate);

            return courses;
        }
    }
}
