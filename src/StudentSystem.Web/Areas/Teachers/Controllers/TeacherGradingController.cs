namespace StudentSystem.Web.Areas.Teachers.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Exam.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Exam.Services.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Attributes;
    using StudentSystem.Web.Infrastructure.Extensions;

    public class TeacherGradingController : BaseTeacherController
    {
        private readonly IStudentCourseService studentCourseService;
        private readonly IExamService examService;

        public TeacherGradingController(
            IStudentCourseService studentCourseService,
            IExamService examService)
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
        [TeacherCourseAssignmentRequired]
        public IActionResult AssignGrade(Guid studentId, Guid courseId)
            => this.View(new ExamBindingModel { StudentId = studentId, CourseId = courseId });

        [HttpPost]
        [TeacherCourseAssignmentRequired]
        [ModelStateValidation]
        public async Task<IActionResult> AssignGrade(ExamBindingModel model)
        {
            this.TempData.Add(await this.examService.CreateAsync(model));

            return this.RedirectToAction(nameof(this.CoursesWithStudents));
        }
    }
}
