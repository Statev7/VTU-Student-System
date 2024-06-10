namespace StudentSystem.Web.Infrastructure.Attributes
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using StudentSystem.Web.Areas.Students.Controllers;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Web.Infrastructure.Constants;

    public class PaymentRedirectionIfNotStudentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isNotStudent = !context.HttpContext.User.IsInRole(StudentRole);

            if (isNotStudent)
            {
                var controllerHelper = context.HttpContext.RequestServices.GetRequiredService<IControllerHelper>();

                context.Result = new RedirectToActionResult(
                    nameof(StudentApplicationController.Apply),
                    controllerHelper.GetName(nameof(StudentApplicationController)),
                    new { area = StudentsArea });

                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
