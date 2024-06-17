namespace StudentSystem.Web.Controllers.ApiControllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;

    public class CoursesApiController : BaseApiController
    {
        private readonly IStudentCourseService studentCourseService;

        public CoursesApiController(IStudentCourseService studentCourseService) 
            => this.studentCourseService = studentCourseService;

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var courseDetails = await this.studentCourseService.GetCourseWithExamDetailsAsync<CourseWithExamDetailsViewModel>(id);

            return this.PartialView("~/Views/Shared/Partials/Courses/_CourseDetailsPartial.cshtml", courseDetails);
        }
    }
}
