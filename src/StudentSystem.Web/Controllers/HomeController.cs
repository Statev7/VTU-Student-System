namespace StudentSystem.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Web.Infrastructure.Helpers.Contracts;
    using StudentSystem.Web.Models;

    public class HomeController : Controller
    {
        private readonly IHomeHelper homeHelper;

        public HomeController(IHomeHelper homeHelper)
            => this.homeHelper = homeHelper;

        public async Task<IActionResult> Index()
        {
            var model = await this.homeHelper.CreateViewModelAsync();

            return this.View(model);
        }

        public IActionResult Privacy() 
            => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
