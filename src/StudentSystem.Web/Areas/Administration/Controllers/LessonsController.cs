namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Lessons.Services.Contracts;
    using StudentSystem.Web.Controllers;
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

        [HttpGet]
        [ModelOrBadRequest]
        public async Task<IActionResult> Update(Guid id)
        {
            var lesson = await this.lessonService.GetByIdAsync<LessonFormBindingModel>(id);

            if (lesson != null)
            {
                lesson.Courses = await this.courseService.GetAllAsync<CourseSelectionItemViewModel>();
            }

            return this.View(lesson);
        }

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> Update(Guid id, LessonFormBindingModel model)
        {
            var result = await this.lessonService.UpdateAsync(id, model);

            this.TempData.Add(result);

            if (result.Succeed)
            {
                return this.RedirectToAction(
                    nameof(TrainingsController.Details), 
                    this.controllerHelper.GetName(nameof(TrainingsController)), 
                    new { Area = "", Id = model.CourseId });
            }

            return this.RedirectToAction(nameof(HomeController.Index), this.controllerHelper.GetName(nameof(HomeController)), new { Area = "" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            this.TempData.Add(await this.lessonService.DeleteAsync(id));

            return this.RedirectToAction(nameof(HomeController.Index), this.controllerHelper.GetName(nameof(HomeController)), new { Area = "" });
        }
    }
}
