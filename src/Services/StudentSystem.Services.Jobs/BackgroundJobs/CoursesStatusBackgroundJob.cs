namespace StudentSystem.Services.Jobs.BackgroundJobs
{
    using System.Threading.Tasks;

    using Quartz;

    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;

    public class CoursesStatusBackgroundJob : IJob
    {
        private readonly ICourseService courseService;

        public CoursesStatusBackgroundJob(ICourseService courseService) 
            => this.courseService = courseService;

        public async Task Execute(IJobExecutionContext context)
        {
            await this.courseService.ChangeActivityStatusAsync();
        }
    }
}
