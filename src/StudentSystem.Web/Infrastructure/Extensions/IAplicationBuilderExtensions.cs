namespace StudentSystem.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Net.Http.Headers;

    using StudentSystem.Data;
    using StudentSystem.Data.Seed;

    public static class IAplicationBuilderExtensions
    {
        private const double StaticFilesCacheTimeInDays = 7;

        public static IApplicationBuilder ConfigureEnvironments(this IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                applicationBuilder.UseMigrationsEndPoint();
            }
            else
            {
                applicationBuilder
                    .UseExceptionHandler("/Home/Error")
                    .UseHsts();
            }

            return applicationBuilder;
        }

        public static IApplicationBuilder ConfigureStaticFiles(this IApplicationBuilder applicationBuilder)
            => applicationBuilder
                .UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = options =>
                    {
                        var headers = options.Context.Response.GetTypedHeaders();

                        headers.CacheControl = new CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = TimeSpan.FromDays(StaticFilesCacheTimeInDays),
                        };

                        headers.Expires = new DateTimeOffset(DateTime.UtcNow.AddDays(StaticFilesCacheTimeInDays));
                    }
                });

        public static IApplicationBuilder ConfigureEndPoints(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "area",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

        public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        public static async Task SeedDataBaseAsync(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            await Launcher.SeedDataBaseAsync(serviceScope.ServiceProvider);
        }
    }
}
