namespace StudentSystem.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Web.Data;

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
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }
    }
}
