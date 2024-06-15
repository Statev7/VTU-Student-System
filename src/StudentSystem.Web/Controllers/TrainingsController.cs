namespace StudentSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Resources.Services.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    public class TrainingsController : Controller
    {
        private const int CoursesPerPage = 6;

        private readonly ICourseService courseService;
        private readonly IResourceService resourceService;
        private readonly IStudentCourseService studentCourseService;
        private readonly IControllerHelper controllerHelper;

        public TrainingsController(
            ICourseService courseService,
            IResourceService resourceService,
            IStudentCourseService studentCourseService,
            IControllerHelper controllerHelper)
        {
            this.courseService = courseService;
            this.resourceService = resourceService;
            this.studentCourseService = studentCourseService;
            this.controllerHelper = controllerHelper;
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

        [HttpGet]
        [ResourceAccess]
        public async Task<IActionResult> DownloadResource(Guid id, Guid courseId)
        {
            var result = await this.resourceService.LoadAsync(id);

            if (!result.Succeed)
            {
                this.TempData.Add(result);

                return this.RedirectToAction(nameof(HomeController.Index), this.controllerHelper.GetName(nameof(HomeController)));
            }

            var file = result.Data;

            return this.File(file.Path, file.ContentType, file.Name);
        }
    }
}
