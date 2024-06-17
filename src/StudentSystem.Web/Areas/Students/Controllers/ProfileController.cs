namespace StudentSystem.Web.Areas.Students.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Students.Services.Contracts;

    using static StudentSystem.Common.Constants.GlobalConstants;

    [Authorize(Roles = StudentRole)]
    public class ProfileController : BaseStudentController
    {
        private readonly IStudentService studentService;

        public ProfileController(IStudentService studentService) 
            => this.studentService = studentService;

        [HttpGet]
        public async Task<IActionResult> Trainings()
        {
            var model = await this.studentService.GetTrainingsInformationAsync();

            return this.View(model);
        }
    }
}
