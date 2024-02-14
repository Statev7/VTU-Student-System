namespace StudentSystem.Services.Data.Features.Students.Services.Contracts
{
    using System.Linq.Expressions;

    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Collections.Contracts;

    public interface IStudentService
    {
        Task<IPageList<TEntity>> GetAllAsync<TEntity>(Expression<Func<Student, bool>> selector, int currentPage);

        Task CreateAsync(BecomeStudentBindingModel model);

        Task<Result> ApproveStudentAsync(string email, bool isApproved);

        Task<bool> IsAppliedAlreadyAsync();
    }
}
