namespace StudentSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;

    public class TrainingsController : Controller
    {
        private const int CoursesPerPage = 6;

        private readonly ICourseService courseService;

        public TrainingsController(ICourseService courseService) 
            => this.courseService = courseService;

        [HttpGet]
        public async Task<IActionResult> Index(CoursesRequestDataModel requestData)
        {
            var courses = await this.courseService.GetAllAsync<CourseViewModel>(requestData, CoursesPerPage);

            return this.View(courses);
        }

        [HttpGet]
        public IActionResult Details() => this.View();
    }
}
