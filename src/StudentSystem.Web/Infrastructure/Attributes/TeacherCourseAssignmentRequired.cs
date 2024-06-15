namespace StudentSystem.Web.Infrastructure.Attributes
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Web.Controllers;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class TeacherCourseAssignmentRequired : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.GetUserId();
            var isValidGuid = Guid.TryParse(context.HttpContext.Request.Query["courseId"].ToString(), out Guid courseId);

            var controller = context.Controller as Controller;

            if (string.IsNullOrEmpty(userId) || !isValidGuid)
            {
                controller.TempData[ErrorNotification] = ErrorMesage;

                this.SetContentResult(context);

                return;
            }

            var teacherService = context.HttpContext.RequestServices.GetRequiredService<ITeacherService>();

            var isTeacherNotLeadTheCourse = !await teacherService.IsLeadTheCourseAsync(userId, courseId);

            if (isTeacherNotLeadTheCourse)
            {
                controller.TempData[ErrorNotification] = TeacherNotPermissionToAssignGradeErrorMessage;

                this.SetContentResult(context);

                return;
            }
        }

        #region Private Methods

        private void SetContentResult(ActionExecutingContext context)
        {
            var controllerHelper = context.HttpContext.RequestServices.GetRequiredService<IControllerHelper>();

            context.Result = new RedirectToActionResult(nameof(HomeController.Index), controllerHelper.GetName(nameof(HomeController)), new { area = "" });
        }

        #endregion
    }
}
