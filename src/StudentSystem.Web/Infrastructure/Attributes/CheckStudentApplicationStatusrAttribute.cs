namespace StudentSystem.Web.Infrastructure.Attributes
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Web.Controllers;

    using static StudentSystem.Web.Infrastructure.Constants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    public class CheckStudentApplicationStatusrAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var studentService = context.HttpContext.RequestServices.GetRequiredService<IStudentService>();

            if (await studentService.IsAppliedAlreadyAsync())
            {
                var controller = context.Controller as Controller;

                controller?.TempData.Add(ErrorNotification, AlreadyAppliedErrorMessage);

                context.Result = new RedirectToActionResult(nameof(HomeController.Index), HomeControllerName, new { area = "" });

                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }

    }
}
