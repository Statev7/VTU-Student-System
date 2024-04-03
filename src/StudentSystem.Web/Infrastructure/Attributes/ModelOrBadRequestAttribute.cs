namespace StudentSystem.Web.Infrastructure.Attributes
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ModelOrBadRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as ViewResult;
            if (result != null && result.Model == null) 
            {
                context.Result = new BadRequestResult();
            }

            base.OnActionExecuted(context);
        }
    }
}
