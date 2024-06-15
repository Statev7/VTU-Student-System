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
            => this.View(new CreateExamBindingModel { StudentId = studentId, CourseId = courseId });

        [HttpPost]
        [TeacherCourseAssignmentRequired]
        [ModelStateValidation]
        public async Task<IActionResult> AssignGrade(CreateExamBindingModel model)
        {
            this.TempData.Add(await this.examService.CreateAsync(model));

            return this.RedirectToAction(nameof(this.CoursesWithStudents), new {courseId = model.CourseId});
        }

        [HttpGet]
        [TeacherCourseAssignmentRequired]
        public async Task<IActionResult> UpdateGrade(Guid examId)
        {
            var exam = await this.examService.GetByIdAsync<UpdateExamBindingModel>(examId);

           return this.View(exam);
        }

        [HttpPost]
        [TeacherCourseAssignmentRequired]
        [ModelStateValidation]
        public async Task<IActionResult> UpdateGrade(Guid examId, Guid courseId, UpdateExamBindingModel model)
        {
            this.TempData.Add(await this.examService.UpdateAsync(examId, model));

            return this.RedirectToAction(nameof(this.CoursesWithStudents), new { courseId });
        }
    }
}
