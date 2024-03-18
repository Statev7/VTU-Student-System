namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class CoursesController : BaseAdminController
    {
        private readonly ICourseService courseService;
        private readonly ITeacherService teacherService;
        private readonly INotificationHelper notificationHelper;

        public CoursesController(
            ICourseService courseService, 
            ITeacherService teacherService, 
            INotificationHelper notificationHelper)
        {
            this.courseService = courseService;
            this.teacherService = teacherService;
            this.notificationHelper = notificationHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CourseFormBidningModel()
            {
                Teachers = await this.teacherService.GetAllAsync<TeacherSelectionItemViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> Create(CourseFormBidningModel model)
        {
            var result = await this.courseService.CreateAsync(model);

            var (notificationType, message) = this.notificationHelper.GenerateNotification(result, SuccessfullyCreatedCourseMessage);
            this.TempData.Add(notificationType, message);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
