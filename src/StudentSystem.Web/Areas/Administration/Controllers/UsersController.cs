namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Teachers.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Services.Data.Features.Users.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Users.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Extensions;

    public class UsersController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly ITeacherService teacherService;

        public UsersController(
            IUserService userService,
            ITeacherService teacherService)
        {
            this.userService = userService;
            this.teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> All(UsersRequestDataModel requestModel)
        {
            var users = await this.userService.GetAllAsync(requestModel);

            return this.View(users);
        }

        [HttpGet]
        [RedirectIfTeacherExists]
        public async Task<IActionResult> CreateTeacher(string email)
        {
            var model = await this.userService.GetByEmailAsync<BecomeTeacherBindingModel>(email);

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidation]
        [RedirectIfTeacherExists]
        public async Task<IActionResult> CreateTeacher(string email, BecomeTeacherBindingModel model)
        {
            this.TempData.Add(await this.teacherService.CreateTeacherAsync(email, model));

            return this.RedirectToAction(nameof(All));
        }
    }
}
