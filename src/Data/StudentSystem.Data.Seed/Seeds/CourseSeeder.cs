namespace StudentSystem.Data.Seed.Seeds
{
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    using StudentSystem.Data.Models.Courses;

    public class CourseSeeder : BaseSeeder<Course>
    {
        public CourseSeeder(IServiceScope serviceScope, string jsonData) 
            : base(serviceScope, jsonData)
        {
        }

        public async override Task SeedAsync()
        {
            if (await this.IsAlreadySeedAsync())
            {
                return;
            }

            var courses = JsonConvert.DeserializeObject<Course[]>(this.JsonData);

            foreach (var course in courses)
            {
                var courseStart = course.StartDate;

                if (DateTime.UtcNow.Date >= courseStart.Date)
                {
                    var duration = (course.EndDate - courseStart.Date).Days;

                    course.StartDate = DateTime.UtcNow.AddMonths(1);
                    course.EndDate = course.StartDate.AddDays(duration);
                }
            }

            await this.DbSet.AddRangeAsync(courses);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
