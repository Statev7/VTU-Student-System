namespace StudentSystem.Web.Areas.Teachers.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;

    public class ScheduleController : BaseTeacherController
    {
        private readonly ITeacherService teacherService;

        public ScheduleController(ITeacherService teacherService) 
            => this.teacherService = teacherService;

        [HttpGet]
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var mySchedule = await this.teacherService.GetMyScheduleAsync<LessonScheduleViewModel>(currentPage);

            return this.View(mySchedule);
        }
    }
}
