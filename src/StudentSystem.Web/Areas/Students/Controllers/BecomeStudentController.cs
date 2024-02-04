namespace StudentSystem.Web.Areas.Students.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using StudentSystem.Services.Data.Features.City.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.City.Services.Contracts;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;

    [Area("Students")]
    [Authorize]
    public class BecomeStudentController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ICityService cityService;

        public BecomeStudentController(IStudentService studentService, ICityService cityService)
        {
            this.studentService = studentService;
            this.cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> Apply()
        {
            var model = new BecomeStudentBindingModel()
            {
                Cities = await this.cityService.GetAllAsync<CityViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(BecomeStudentBindingModel model)
        {
            var result = await this.studentService.CreateAsync(model);

            if (!result.Succeed)
            {
                return this.BadRequest();
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
