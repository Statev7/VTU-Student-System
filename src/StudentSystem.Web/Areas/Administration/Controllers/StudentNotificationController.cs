namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    public class StudentNotificationController : BaseAdminController
    {
        private readonly ICourseService courseService;
        private readonly IStudentCourseService studentCourseService;
        private readonly IControllerHelper controllerHelper;

        public StudentNotificationController(
            ICourseService courseService,
            IStudentCourseService studentCourseService,
            IControllerHelper controllerHelper)
        {
            this.courseService = courseService;
            this.studentCourseService = studentCourseService;
            this.controllerHelper = controllerHelper;
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail()
        {
            var model = new EmailInformationBindingModel()
            {
                Courses = await courseService.GetAllAsync<CourseSelectionItemViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> SendEmail(EmailInformationBindingModel model)
        {
            this.TempData.Add(await studentCourseService.SendInformationAsync(model));

            return this.RedirectToAction(nameof(DashboardController.Index), controllerHelper.GetName(nameof(DashboardController)));
        }
    }
}
