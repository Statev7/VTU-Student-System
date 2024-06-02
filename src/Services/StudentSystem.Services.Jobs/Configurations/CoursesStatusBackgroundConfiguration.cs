namespace StudentSystem.Services.Jobs.Configurations
{
    using Microsoft.Extensions.Options;

    using Quartz;
    using StudentSystem.Common;
    using StudentSystem.Services.Jobs.BackgroundJobs;

    public class CoursesStatusBackgroundConfiguration : IConfigureOptions<QuartzOptions>
    {
        private readonly ApplicationSettings applicationSettings;

        public CoursesStatusBackgroundConfiguration(IOptions<ApplicationSettings> options)
            => this.applicationSettings = options.Value;

        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(CoursesStatusBackgroundJob));

            options
                .AddJob<CoursesStatusBackgroundJob>(options => options.WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithCronSchedule(this.applicationSettings.CoursesStatusCronSchedule));
        }
    }
}
