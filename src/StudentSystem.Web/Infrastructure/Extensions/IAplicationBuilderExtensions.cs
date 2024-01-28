namespace StudentSystem.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;

    public static class IAplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureEnvironment(this IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
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

        public static IApplicationBuilder ConfigureEndPoints(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
    }
}
