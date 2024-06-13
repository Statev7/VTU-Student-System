namespace StudentSystem.Web.Areas.Teachers.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using StudentSystem.Services.Data.Features.Exam.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Exam.Services.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.RequestDataModels;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Extensions;

    using static StudentSystem.Web.Infrastructure.Constants;
    using static StudentSystem.Common.Constants.GlobalConstants;

    [Area(TeachersArea)]
    [Authorize(Roles = TeacherRole)]
    public class DashboardController : Controller
    {
        private readonly IStudentCourseService studentCourseService;
        private readonly IExamService examService;

        public DashboardController(IStudentCourseService studentCourseService, IExamService examService)
        {
            this.studentCourseService = studentCourseService;
            this.examService = examService;
        }

        [HttpGet]
        public async Task<IActionResult> CoursesWithStudents(StudentsInCourseRequestData requestData)
        {
            var students = await this.studentCourseService.GetStudentsByCourseAsync(requestData);

            return this.View(students);
        }

        [HttpGet]
        public IActionResult AssignGrade(Guid studentId, Guid courseId) 
            => this.View(new ExamBindingModel { StudentId = studentId, CourseId = courseId });

        [HttpPost]
        [ModelStateValidation]
        public async Task<IActionResult> AssignGrade(ExamBindingModel model)
        {
            this.TempData.Add(await this.examService.CreateAsync(model));

            return this.RedirectToAction(nameof(this.CoursesWithStudents));
        }
    }
}
