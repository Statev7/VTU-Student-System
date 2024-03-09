namespace StudentSystem.Web
{
    using StudentSystem.Web.Infrastructure.Extensions;

    using static StudentSystem.Web.Infrastructure.Constants;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString(DefaultConnectionString);

            builder
                .Services
                .ConfigureDataBase(connectionString, builder.Environment)
                .ConfigureIdentity()
                .RegisterServices()
                .RegisterRepositories()
                .RegisterHelpers()
                .RegisterAutoMapper()
                .RegisterEmailSender()
                .AddHttpContextAccessor()
                .AddMemoryCache()
                .ConfigureApplicationSettings(builder.Configuration)
                .ConfigureControllersWithViews();

            var app = builder.Build();

            app
                .ConfigureEnvironments(app.Environment)
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .ConfigureEndPoints();

            if (app.Environment.IsDevelopment())
            {
                app.MigrateDatabaseAsync().GetAwaiter().GetResult();

                app.SeedDataBaseAsync().GetAwaiter().GetResult();
            }

            app.Run();
        }
    }
}