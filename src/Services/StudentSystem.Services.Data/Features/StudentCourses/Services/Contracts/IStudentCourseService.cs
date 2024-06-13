namespace StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts
{
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Students.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface IStudentCourseService
    {
        Task<ListStudentsViewModel> GetStudentsByCourseAsync(StudentsInCourseRequestData requestData);

        Task<bool> IsUserRegisteredInCourseAsync(Guid courseId, string userId);

        Task AddStudentToCourseAsync(Guid courseId);

        Task<Result> SendInformationAsync(EmailInformationBindingModel model);

        Task<bool> IsExistAsync(Guid studentId, Guid courseId);

        Task<bool> HasGradeAsync(Guid studentId, Guid courseId);
    }
}
