namespace StudentSystem.Services.Jobs.Configurations
{
    using Microsoft.Extensions.Options;

    using Quartz;

    using StudentSystem.Common;
    using StudentSystem.Services.Jobs.BackgroundJobs;

    public class StudentsStatusBackgroundConfiguration : IConfigureOptions<QuartzOptions>
    {
        private readonly ApplicationSettings applicationSettings;

        public StudentsStatusBackgroundConfiguration(IOptions<ApplicationSettings> options) 
            => this.applicationSettings = options.Value;

        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(StudentsStatusBackgroundJob));

            options
                .AddJob<StudentsStatusBackgroundJob>(options => options.WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithCronSchedule(this.applicationSettings.StudentsStatusCronSchedule));
        }
    }
}
