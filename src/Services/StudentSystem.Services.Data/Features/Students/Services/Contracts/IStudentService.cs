namespace StudentSystem.Services.Data.Features.Students.Services.Contracts
{
    using System.Linq.Expressions;

    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface IStudentService
    {
        Task<IPageList<TEntity>> GetAllAsync<TEntity>(Expression<Func<Student, bool>> selector, int currentPage);

        Task<Result> CreateAsync(BecomeStudentBindingModel bindingModel);

        Task<Result> ApproveStudentAsync(string email, bool isApproved);

        Task<bool> IsAppliedAlreadyAsync();

        Task<Guid> GetIdByUserIdAsync(string userId);
    }
}
