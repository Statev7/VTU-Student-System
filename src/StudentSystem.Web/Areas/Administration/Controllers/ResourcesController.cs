﻿namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Lessons.Services.Contracts;
    using StudentSystem.Services.Data.Features.Resources.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Resources.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    public class ResourcesController : BaseAdminController
    {
        private readonly IResourceService resourceService;
        private readonly ILessonService lessonService;
        private readonly IControllerHelper controllerHelper;

        public ResourcesController(
            IResourceService resourceService,
            ILessonService lessonService,
            IControllerHelper controllerHelper)
        {
            this.resourceService = resourceService;
            this.lessonService = lessonService;
            this.controllerHelper = controllerHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ResourceBindingModel
            {
                Lessons = await this.lessonService.GetAllAsync<LessonSelectionItemViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> Create(ResourceBindingModel model)
        {
            this.TempData.Add(await this.resourceService.CreateAsync(model));

            return this.RedirectToAction(nameof(CoursesController.All), this.controllerHelper.GetName(nameof(CoursesController)));
        }
    }
}