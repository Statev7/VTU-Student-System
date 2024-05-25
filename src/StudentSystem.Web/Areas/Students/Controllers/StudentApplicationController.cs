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
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Web.Infrastructure.Constants;

    [Area(StudentsArea)]
    public class StudentApplicationController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ICityService cityService;
        private readonly IControllerHelper controllerHelper;

        public StudentApplicationController(
            IStudentService studentService, 
            ICityService cityService,
            IControllerHelper controllerHelper)
        {
            this.studentService = studentService;
            this.cityService = cityService;
            this.controllerHelper = controllerHelper;
        }

        [HttpGet]
        [Authorize(Roles = GuestRole)]
        [CheckStudentApplicationStatus]
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
            this.TempData.Add(await this.studentService.CreateAsync(model));

            return this.RedirectToAction(nameof(HomeController.Index), this.controllerHelper.GetName(nameof(HomeController)), new { area = "" });
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
            this.TempData.Add(await this.studentService.ApproveStudentAsync(email, isApproved));

            return this.RedirectToAction(nameof(PendingStudents));
        }
    }
}
