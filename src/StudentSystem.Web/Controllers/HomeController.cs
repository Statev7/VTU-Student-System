namespace StudentSystem.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Enums;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Web.Models;

    public class HomeController : Controller
    {
        private const int CoursesToDisplay = 8;

        private readonly ICourseService courseService;

        public HomeController(ICourseService courseService) 
            => this.courseService = courseService;

        public async Task<IActionResult> Index()
        {
            var requestData = new CoursesRequestDataModel() { CurrentPage = 1, OrderBy = CoursesOrderOptions.DescStartDate };

            var coursesPageList = await this.courseService.GetAllAsync<LatestCourseViewModel>(requestData, CoursesToDisplay);

            var model = new HomeViewModel()
            {
                LatestCourses = coursesPageList?.PageList?.Entities,
            };

            return this.View(model);
        }
         

        public IActionResult Privacy() 
            => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
