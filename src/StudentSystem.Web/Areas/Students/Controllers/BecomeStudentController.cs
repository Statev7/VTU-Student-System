﻿namespace StudentSystem.Web.Areas.Students.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.City.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.City.Services.Contracts;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Web.Controllers;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    [Area("Students")]
    [Authorize(Roles = GuestRole)]
    public class BecomeStudentController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ICityService cityService;
        private readonly INotificationHelper notificationHelper;

        public BecomeStudentController(
            IStudentService studentService, 
            ICityService cityService,
            INotificationHelper notificationHelper)
        {
            this.studentService = studentService;
            this.cityService = cityService;
            this.notificationHelper = notificationHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Apply()
        {
            if(await this.studentService.IsAppliedAlreadyAsync())
            {
                this.TempData[ErrorNotification] = AlreadyAppliedErrorMessage;

                return this.RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
            }

            var model = new BecomeStudentBindingModel()
            {
                Cities = await this.cityService.GetAllAsync<CityViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> Apply(BecomeStudentBindingModel model)
        {
            var result = await this.studentService.CreateAsync(model);

            var (notificationType, message) = this.notificationHelper.GenerateNotification(result, SuccessfullyAppliedMessage);

            this.TempData[notificationType] = message;

            return this.RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
        }
    }
}
