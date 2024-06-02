namespace StudentSystem.Services.Jobs.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;

    using Quartz;
    using StudentSystem.Services.Jobs.Configurations;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterJobs(this IServiceCollection services)
        {
            services.AddQuartz();

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<StudentsStatusBackgroundConfiguration>();

            return services;
        }
    }
}
