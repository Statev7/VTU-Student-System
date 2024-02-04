namespace StudentSystem.Web
{
    using StudentSystem.Web.Infrastructure.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder
                .Services
                .ConfigureDataBase(connectionString, builder.Environment)
                .ConfigureIdentity()
                .AddHttpContextAccessor()
                .RegisterServices()
                .RegisterRepositories()
                .RegisterHelpers()
                .RegisterAutoMapper()
                .RegisterEmailSender()
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