namespace StudentSystem.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Common;
    using StudentSystem.Data;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Messaging;
    using StudentSystem.Services.Messaging.Senders;

    public static class IServiceCollectionExtensions
    {
        private const string ConnectionStringNotFoundErrorMessage = "Connection string '{0}' not found.";

        public static IServiceCollection ConfigureDataBase(
            this IServiceCollection services, 
            string connectionString,
            IWebHostEnvironment environment)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                var error = string.Format(ConnectionStringNotFoundErrorMessage, connectionString);

                throw new InvalidOperationException(error);
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            if (environment.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }

            return services;
        }

        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services
                .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection ConfigureEmailSender(this IServiceCollection services)
            => services.AddTransient<IEmailSender, SendGridEmailSender>();

        public static IServiceCollection ConfigureApplicationSettings(this IServiceCollection services, ConfigurationManager configuration)
            => services
                .Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));
    }
}
