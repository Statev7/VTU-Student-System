namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Services.Data.Features.Users.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Users.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    public class UsersController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly ITeacherService teacherService;
        private readonly INotificationHelper notificationHelper;

        public UsersController(
            IUserService userService,
            ITeacherService teacherService,
            INotificationHelper notificationHelper)
        {
            this.userService = userService;
            this.teacherService = teacherService;
            this.notificationHelper = notificationHelper;
        }

        [HttpGet]
        public async Task<IActionResult> All(UsersRequestDataModel requestModel)
        {
            var users = await this.userService.GetAllAsync(requestModel);

            return this.View(users);
        }

        [HttpGet]
        [RedirectIfTeacherExistsAttribute]
        public async Task<IActionResult> CreateTeacher(string email)
        {
            var model = await this.userService.GetByEmailAsync<BecomeTeacherBindingModel>(email);

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidation]
        [RedirectIfTeacherExistsAttribute]
        public async Task<IActionResult> CreateTeacher(string email, BecomeTeacherBindingModel model)
        {
            var result = await this.teacherService.CreateTeacherAsync(email, model);

            var (notificationType, message) = this.notificationHelper.GenerateNotification(result, SuccessfullyCreatedTeacherMessage);
            this.TempData.Add(notificationType, message);

            return this.RedirectToAction(nameof(All));
        }
    }
}
