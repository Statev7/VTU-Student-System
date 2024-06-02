namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Web.Controllers;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    public class CoursesController : BaseAdminController
    {
        private const int CoursesPerPage = 5;

        private readonly ICourseService courseService;
        private readonly ITeacherService teacherService;
        private readonly IControllerHelper controllerHelper;

        public CoursesController(
            ICourseService courseService, 
            ITeacherService teacherService,
            IControllerHelper controllerHelper)
        {
            this.courseService = courseService;
            this.teacherService = teacherService;
            this.controllerHelper = controllerHelper;
        }

        [HttpGet]
        public async Task<IActionResult> All(CoursesRequestDataModel requestData)
        {
            var courses = await this.courseService.GetAllAsync<CourseManagementViewModel>(
                requestData, 
                CoursesPerPage, 
                includeExpiredCourses: true, 
                includeAlreadyStarted: true);

            return this.View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CourseFormBindingModel()
            {
                Teachers = await this.teacherService.GetAllAsync<TeacherSelectionItemViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> Create(CourseFormBindingModel model)
        {
            this.TempData.Add(await this.courseService.CreateAsync(model));

            return this.RedirectToAction(nameof(TrainingsController.Index), this.controllerHelper.GetName(nameof(TrainingsController)), new { area = "" });
        }

        [HttpGet]
        [ModelOrBadRequest]
        public async Task<IActionResult> Update(Guid id)
        {
            var courseToUpdate = await this.courseService.GetByIdAsync<CourseFormBindingModel>(id);
            if (courseToUpdate != null)
            {
                courseToUpdate.Teachers = await this.teacherService.GetAllAsync<TeacherSelectionItemViewModel>();
            }

            return this.View(courseToUpdate);
        }

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> Update(Guid id, CourseFormBindingModel model)
        {
            this.TempData.Add(await this.courseService.UpdateAsync(id, model));

            return this.RedirectToAction(nameof(TrainingsController.Index), this.controllerHelper.GetName(nameof(TrainingsController)), new { area = "" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            this.TempData.Add(await this.courseService.DeleteAsync(id));

            return this.RedirectToAction(nameof(TrainingsController.Index), this.controllerHelper.GetName(nameof(TrainingsController)), new { area = "" });
        }
    }
}
