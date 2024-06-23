namespace StudentSystem.Web.Controllers.ApiControllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Lessons.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;

    public class LessonsApiController : BaseApiController
    {
        private readonly ILessonService lessonService;

        public LessonsApiController(ILessonService lessonService)
            => this.lessonService = lessonService;

        [HttpGet]
        [Route("{id}")]
        [ModelOrBadRequest]
        public async Task<IActionResult> Details(Guid id)
        {
            var lesson = await lessonService.GetDetailsAsync(id);

            return this.PartialView("~/Views/Shared/Partials/Lessons/_LessonDetailsPartial.cshtml", lesson);
        }
    }
}
