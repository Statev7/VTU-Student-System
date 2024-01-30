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
                .ConfigureEmailSender()
                .ConfigureApplicationSettings(builder.Configuration)
                .AddControllersWithViews();

            var app = builder.Build();

            app
                .ConfigureEnvironment(app.Environment)
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .ConfigureEndPoints();

            app.Run();
        }
    }
}