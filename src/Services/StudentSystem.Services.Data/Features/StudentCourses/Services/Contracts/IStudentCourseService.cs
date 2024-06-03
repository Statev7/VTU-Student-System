namespace StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts
{
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface IStudentCourseService
    {
        Task<bool> IsUserRegisteredInCourseAsync(Guid courseId, string userId);

        Task AddStudentToCourseAsync(Guid courseId);

        Task<Result> SendInformationAsync(EmailInformationBindingModel model);
    }
}
