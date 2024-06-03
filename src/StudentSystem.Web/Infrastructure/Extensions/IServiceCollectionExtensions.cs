namespace StudentSystem.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Stripe;

    using StudentSystem.Common;
    using StudentSystem.Common.Infrastructure.Cache.Services.Contracts;
    using StudentSystem.Common.Infrastructure.Cache.Services.Implementation;
    using StudentSystem.Data;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Data.Repositories;
    using StudentSystem.Services.Data.Features.City.Services.Contracts;
    using StudentSystem.Services.Data.Features.City.Services.Implementation;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Courses.Services.Implementation;
    using StudentSystem.Services.Data.Features.ImageFiles.Services.Contracts;
    using StudentSystem.Services.Data.Features.ImageFiles.Services.Implementation;
    using StudentSystem.Services.Data.Features.Lessons.Services.Contracts;
    using StudentSystem.Services.Data.Features.Lessons.Services.Implementation;
    using StudentSystem.Services.Data.Features.Payments.Services.Contracts;
    using StudentSystem.Services.Data.Features.Payments.Services.Implementation;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Implementation;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Services.Data.Features.Students.Services.Implementation;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Services.Data.Features.Teachers.Services.Implementation;
    using StudentSystem.Services.Data.Features.Users.Services.Contracts;
    using StudentSystem.Services.Data.Features.Users.Services.Implementation;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Services.Implementation;
    using StudentSystem.Services.Messaging;
    using StudentSystem.Services.Messaging.Senders;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;
    using StudentSystem.Web.Infrastructure.Helpers.Implementation;

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
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;

                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
            => services
                .AddSingleton<ICacheService, CacheService>()
                .AddTransient<ICityService, CityService>()
                .AddTransient<IStudentService, StudentService>()
                .AddTransient<ICurrentUserService, CurrentUserService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<ITeacherService, TeacherService>()
                .AddTransient<ICourseService, CourseService>()
                .AddTransient<IImageFileService, ImageFileService>()
                .AddTransient<IPaymentService, PaymentService>()
                .AddTransient<IStudentCourseService, StudentCourseService>()
                .AddTransient<ILessonService, LessonService>();

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
            => services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));

        public static IServiceCollection RegisterHelpers(this IServiceCollection services)
            => services
                .AddTransient<IControllerHelper, ControllerHelper>()
                .AddTransient<IHomeHelper, HomeHelper>();

        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
            => services.AddAutoMapper(typeof(BaseMappingProfile).Assembly);

        public static IServiceCollection RegisterEmailSender(this IServiceCollection services)
            => services.AddTransient<IEmailSender, SendGridEmailSender>();

        public static IServiceCollection ConfigureApplicationSettings(this IServiceCollection services, ConfigurationManager configuration)
            => services.Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));

        public static IServiceCollection ConfigureStripe(this IServiceCollection services, ConfigurationManager configuration)
        {
            StripeConfiguration.ApiKey = configuration["ApplicationSettings:StripeSecretKey"];

            return services;
        }

        public static void ConfigureControllersWithViews(this IServiceCollection services)
            => services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
    }
}
