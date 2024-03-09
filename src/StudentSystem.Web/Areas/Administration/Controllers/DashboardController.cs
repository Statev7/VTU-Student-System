namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : BaseAdminController
    {
        public IActionResult Index() => this.View();
    }
}
