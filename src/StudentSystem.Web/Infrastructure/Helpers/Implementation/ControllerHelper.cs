namespace StudentSystem.Web.Infrastructure.Helpers.Implementation
{
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    public class ControllerHelper : IControllerHelper
    {
        private const string Controller = "Controller";

        public string GetName(string controller)
            => controller.Replace(Controller, "");
    }
}
