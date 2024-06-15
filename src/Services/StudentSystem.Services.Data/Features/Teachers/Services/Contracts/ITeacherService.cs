namespace StudentSystem.Services.Data.Features.Teachers.Services.Contracts
{
    using AngleSharp.Dom;

    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface ITeacherService
    {
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();

        Task<IEnumerable<CourseSelectionItemViewModel>> GetMyCoursesAsync();

        Task<IPageList<TEntity>> GetMyScheduleAsync<TEntity>(int currentPage)
            where TEntity : class;

        Task<Guid> GetIdByUserId(string userId);

        Task<Result> CreateTeacherAsync(string userEmail, BecomeTeacherBindingModel bindingModel);

        Task<bool> IsLeadTheCourseAsync(string userId, Guid courseId);
    }
}
