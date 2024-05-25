namespace StudentSystem.Web.Infrastructure.Attributes
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Web.Controllers;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class CheckStudentApplicationStatusAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var studentService = context.HttpContext.RequestServices.GetRequiredService<IStudentService>();
            var controllerHelper = context.HttpContext.RequestServices.GetRequiredService<IControllerHelper>();

            if (await studentService.IsAppliedAlreadyAsync())
            {
                var controller = context.Controller as Controller;

                controller?.TempData.Add(ErrorNotification, AlreadyAppliedErrorMessage);

                context.Result = new RedirectToActionResult(nameof(HomeController.Index), controllerHelper.GetName(nameof(HomeController)), new { area = "" });

                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }

    }
}
