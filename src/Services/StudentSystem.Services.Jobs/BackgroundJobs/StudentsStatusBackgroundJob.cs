namespace StudentSystem.Services.Jobs.BackgroundJobs
{
    using Quartz;

    using StudentSystem.Services.Data.Features.Students.Services.Contracts;

    public class StudentsStatusBackgroundJob : IJob
    {
        private readonly IStudentService studentService;

        public StudentsStatusBackgroundJob(IStudentService studentService) 
            => this.studentService = studentService;

        public async Task Execute(IJobExecutionContext context)
        {
            await this.studentService.ChangeActivityStatusAsync();
        }
    }
}
