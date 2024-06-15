namespace StudentSystem.Services.Data.Features.Exam.Services.Implementation
{
    using AutoMapper;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Exam.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Exam.Services.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class ExamService : BaseService<Exam>, IExamService
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IStudentCourseService studentCourseService;
        private readonly ITeacherService teacherService;

        public ExamService(
            IRepository<Exam> repository, 
            IMapper mapper,
            ICurrentUserService currentUserService,
            IStudentCourseService studentCourseService,
            ITeacherService teacherService) 
            : base(repository, mapper)
        {
            this.currentUserService = currentUserService;
            this.studentCourseService = studentCourseService;
            this.teacherService = teacherService;
        }

        public async Task<Result> CreateAsync(ExamBindingModel model)
        {
            var result = await this.ValidateExamAsync(model.StudentId, model.CourseId);

            if (!result.Succeed) 
            {
                return Result.Failure(result.Message);
            }

            var examToCreate = this.Mapper.Map<Exam>(model);

            await this.Repository.AddAsync(examToCreate);

            await this.Repository.SaveChangesAsync();

            return Result.Success(SuccessfullyAssignedGradeMessage);
        }

        private async Task<Result> ValidateExamAsync(Guid studentId, Guid courseId)
        {
            var isStudentCourseMapNotExist = !await this.studentCourseService.IsExistAsync(studentId, courseId);

            if (isStudentCourseMapNotExist)
            {
                return Result.Failure(StudentCourseRelationNotFoundErrorMessage);
            }

            var isAlreadyHasAGrade = await this.studentCourseService.HasGradeAsync(studentId, courseId);

            if (isAlreadyHasAGrade)
            {
                return Result.Failure(StudentAlreadyHadGradeErrorMessage);
            }

            var isTeacherNotLeadTheCourse = !await this.teacherService.IsLeadTheCourseAsync(this.currentUserService.GetUserId(), courseId);

            if (isTeacherNotLeadTheCourse)
            {
                return Result.Failure(TeacherNotPermissionToAssignGradeErrorMessage);
            }

            return true;
        }
    }
}
