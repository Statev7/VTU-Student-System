namespace StudentSystem.Web.Infrastructure.Attributes
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using StudentSystem.Services.Data.Features.Resources.Services.Contracts;
    using StudentSystem.Web.Controllers;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class ResourceAccessAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isValidGuid = Guid.TryParse(context.HttpContext.Request.Query["courseId"].ToString(), out Guid courseId);

            var user = context.HttpContext.User;
            var controller = context.Controller as Controller;

            if (!isValidGuid && user == null)
            {
                controller.TempData[ErrorNotification] = ErrorMesage;

                this.SetContentResult(context);

                return;
            }

            var resourceService = context.HttpContext.RequestServices.GetRequiredService<IResourceService>();

            var isUserNotHaveAccess = !await resourceService.CurrentUserHasAccessToDonwloadAsync(courseId);

            if (isUserNotHaveAccess)
            {
                controller.TempData[ErrorNotification] = ResourceNotAccessErrorMessage;

                this.SetContentResult(context);

                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }

        #region Private Methods

        private void SetContentResult(ActionExecutingContext context)
        {
            var controllerHelper = context.HttpContext.RequestServices.GetRequiredService<IControllerHelper>();

            context.Result = new RedirectToActionResult(
                    nameof(TrainingsController.Index),
                    controllerHelper.GetName(nameof(TrainingsController)),
                    new { });
        }

        #endregion
    }
}
