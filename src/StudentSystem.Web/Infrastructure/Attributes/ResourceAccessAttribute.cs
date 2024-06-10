namespace StudentSystem.Web.Infrastructure.Attributes
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Web.Controllers;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class ResourceAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isValidGuid = Guid.TryParse(context.HttpContext.Request.Query["courseId"].ToString(), out Guid courseId);

            var user = context.HttpContext.User;

            if (!isValidGuid && user == null)
            {
                this.SetContentResult(context, ErrorMesage);

                return;
            }

            var isUserNotHaveAccess = Task.Run(async () =>
            {
                return !await IsUserHaveAccessAsync(context, user, courseId);
            })
                .GetAwaiter()
                .GetResult();

            if (isUserNotHaveAccess)
            {
                this.SetContentResult(context, ResourceNotAccessErrorMessage);

                return;
            }
        }

        #region Private Methods

        private void SetContentResult(ActionExecutingContext context, string errorMessage)
        {
            var controller = context.Controller as Controller;

            controller.TempData[ErrorNotification] = errorMessage;

            var controllerHelper = context.HttpContext.RequestServices.GetRequiredService<IControllerHelper>();

            context.Result = new RedirectToActionResult(
                    nameof(TrainingsController.Index),
                    controllerHelper.GetName(nameof(TrainingsController)),
                    new { });
        }

        private async Task<bool> IsUserHaveAccessAsync(ActionExecutingContext context, ClaimsPrincipal user, Guid courseId)
        {
            var studentCourseService = context.HttpContext.RequestServices.GetRequiredService<IStudentCourseService>();
            var teacherCourseService = context.HttpContext.RequestServices.GetRequiredService<ITeacherService>();

            var userId = user.GetUserId();

            var isUserInCourse = user.IsStudent() && await studentCourseService.IsUserRegisteredInCourseAsync(courseId, userId);

            var isTeacherLeadTheCourse = user.IsTeacher() && await teacherCourseService.IsLeadTheCourseAsync(userId, courseId);

            var haveAccess = isUserInCourse || isTeacherLeadTheCourse || user.IsAdmin();

            return haveAccess;
        }

        #endregion
    }
}
