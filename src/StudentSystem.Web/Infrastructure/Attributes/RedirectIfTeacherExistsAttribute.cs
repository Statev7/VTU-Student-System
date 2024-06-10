namespace StudentSystem.Web.Infrastructure.Attributes
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using StudentSystem.Services.Data.Features.Users.Services.Contracts;
    using StudentSystem.Web.Areas.Administration.Controllers;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Web.Infrastructure.Constants;
    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    public class RedirectIfTeacherExistsAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

            context.ActionArguments.TryGetValue("email", out var email);

            if (email != null && await userService.IsUserWithRoleExistAsync(email.ToString(), TeacherRole))
            {
                var controllerHelper = context.HttpContext.RequestServices.GetRequiredService<IControllerHelper>();

                var controller = context.Controller as Controller;

                controller.TempData[ErrorNotification] = AlreadyATeacherErrorMessage;

                context.Result = new RedirectToActionResult(nameof(UsersController.All), controllerHelper.GetName(nameof(UsersController)), AdministrationArea);

                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
