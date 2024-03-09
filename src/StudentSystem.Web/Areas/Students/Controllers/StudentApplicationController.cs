namespace StudentSystem.Web.Areas.Students.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.City.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.City.Services.Contracts;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Students.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Web.Controllers;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Web.Infrastructure.Constants;
    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    [Area(StudentsArea)]
    public class StudentApplicationController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ICityService cityService;
        private readonly INotificationHelper notificationHelper;

        public StudentApplicationController(
            IStudentService studentService, 
            ICityService cityService,
            INotificationHelper notificationHelper)
        {
            this.studentService = studentService;
            this.cityService = cityService;
            this.notificationHelper = notificationHelper;
        }

        [HttpGet]
        [Authorize(Roles = GuestRole)]
        [CheckStudentApplicationStatusrAttribute]
        public async Task<IActionResult> Apply()
        {
            var model = new BecomeStudentBindingModel() { Cities = await this.cityService.GetAllAsync<CityViewModel>() };

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidation]
        [Authorize(Roles = GuestRole)]
        public async Task<IActionResult> Apply(BecomeStudentBindingModel model)
        {
            await this.studentService.CreateAsync(model);

            this.TempData.Add(SuccessNotification, SuccessfullyAppliedMessage);

            return this.RedirectToAction(nameof(HomeController.Index), HomeControllerName, new { area = "" });
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> PendingStudents(int currentPage = 1)
        {
            var pendingStudents = await this.studentService.GetAllAsync<PendingStudentViewModel>(s => s.IsApplied && !s.IsApproved, currentPage);

            return this.View(pendingStudents);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> ApproveStudent(string email, bool isApproved)
        {
            var result = await this.studentService.ApproveStudentAsync(email, isApproved);

            var (notificationType, message) = this.notificationHelper.GenerateNotification(result, SuccesfullyAprovedOperationMessage);
            this.TempData.Add(notificationType, message);

            return this.RedirectToAction(nameof(PendingStudents));
        }
    }
}
