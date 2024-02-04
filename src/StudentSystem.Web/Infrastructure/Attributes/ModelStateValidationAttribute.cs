namespace StudentSystem.Web.Infrastructure.Attributes
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class ModelStateValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.ModelState;

            if (!modelState.IsValid)
            {
                var controller = filterContext.Controller as Controller;

                var errors = modelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToArray();

                var errorMessage = errors.Length > 1 ? string.Join("<br>", errors) : errors[0];

                controller.TempData[ErrorNotification] = errorMessage;

                filterContext.ActionDescriptor.RouteValues.TryGetValue("action", out string action);
                filterContext.Result = controller.RedirectToAction(action);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
