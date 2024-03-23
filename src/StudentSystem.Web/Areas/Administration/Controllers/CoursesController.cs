﻿namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Extensions;

    public class CoursesController : BaseAdminController
    {
        private readonly ICourseService courseService;
        private readonly ITeacherService teacherService;

        public CoursesController(
            ICourseService courseService, 
            ITeacherService teacherService)
        {
            this.courseService = courseService;
            this.teacherService = teacherService;
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
            this.TempData.Add(await this.courseService.CreateAsync(model));

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [ModelOrBadRequestAttribute]
        public async Task<IActionResult> Update(Guid id)
        {
            var courseToUpdate = await this.courseService.GetByIdAsync<CourseFormBidningModel>(id);
            if (courseToUpdate != null)
            {
                courseToUpdate.Teachers = await this.teacherService.GetAllAsync<TeacherSelectionItemViewModel>();
            }

            return this.View(courseToUpdate);
        }

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> Update(Guid id, CourseFormBidningModel model)
        {
            this.TempData.Add(await this.courseService.UpdateAsync(id, model));

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            this.TempData.Add(await this.courseService.DeleteAsync(id));

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
