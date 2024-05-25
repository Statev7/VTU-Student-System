namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Lessons.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    public class LessonsController : BaseAdminController
    {
        private readonly ILessonService lessonService;
        private readonly ICourseService courseService;
        private readonly IControllerHelper controllerHelper;

        public LessonsController(
            ILessonService lessonService,
            ICourseService courseService,
            IControllerHelper controllerHelper)
        {
            this.lessonService = lessonService;
            this.courseService = courseService;
            this.controllerHelper = controllerHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new LessonFormBindingModel()
            {
                Courses = await this.courseService.GetAllAsync<CourseSelectionItemViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> Create(LessonFormBindingModel model)
        {
            this.TempData.Add(await this.lessonService.CreateAsync(model));

            return this.RedirectToAction(nameof(CoursesController.All), this.controllerHelper.GetName(nameof(CoursesController)));
        }
    }
}
