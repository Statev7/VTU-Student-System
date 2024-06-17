namespace StudentSystem.Services.Data.Features.Students.Services.Contracts
{
    using System.Linq.Expressions;

    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Students.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Contracts;

    public interface IStudentService : IActivityStatusChanger
    {
        Task<IPageList<TEntity>> GetAllAsync<TEntity>(Expression<Func<Student, bool>> selector, int currentPage) 
            where TEntity : class;

        Task<IEnumerable<TEntity>> GetScheduleAsync<TEntity>(string userId) 
            where TEntity : class;

        Task<IEnumerable<TEntity>> GetCoursesAsync<TEntity>(string userId) 
            where TEntity : class;

        Task<StudentTrainingsInformationViewModel> GetTrainingsInformationAsync();

        Task<Result> CreateAsync(BecomeStudentBindingModel bindingModel);

        Task<Result> ApproveStudentAsync(string email, bool isApproved);

        Task<bool> IsAppliedAlreadyAsync();

        Task<Guid> GetIdByUserIdAsync(string userId);

        Task<Result> SetActiveStatusAsync(Guid id, bool isActive);

        Task<bool> IsActiveAsync(string userId);
    }
}
