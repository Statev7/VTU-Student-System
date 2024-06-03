namespace StudentSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;

    public class TrainingsController : Controller
    {
        private const int CoursesPerPage = 6;

        private readonly ICourseService courseService;
        private readonly IStudentCourseService studentCourseService;

        public TrainingsController(ICourseService courseService, IStudentCourseService studentCourseService)
        {
            this.courseService = courseService;
            this.studentCourseService = studentCourseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CoursesRequestDataModel requestData)
        {
            var courses = await this.courseService.GetAllAsync<CourseViewModel>(requestData, CoursesPerPage);

            return this.View(courses);
        }

        [HttpGet]
        [ModelOrBadRequest]
        public async Task<IActionResult> Details(Guid id)
        {
            var course = await this.courseService.GetDetailsAsync(id);

            var userId = this.User.GetUserId();

            if (course != null && !string.IsNullOrEmpty(userId))
            {
                course.IsUserAlreadyInCourse = await this.studentCourseService.IsUserRegisteredInCourseAsync(id, userId);
            }

            return this.View(course);
        }
    }
}
