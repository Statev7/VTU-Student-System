namespace StudentSystem.Services.Data.Infrastructure.Services.Contracts
{
    public interface IStudentCourseService
    {
        Task<bool> IsUserRegisteredInCourseAsync(Guid courseId, string userId);

        Task AddStudentToCourseAsync(Guid courseId);
    }
}
