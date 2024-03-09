namespace StudentSystem.Services.Data.Features.Users.Services.Contracts
{
    using System.Linq.Expressions;

    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Users.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Users.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface IUserService
    {
        Task<ApplicationUser> GetByEmailAsync(string email);

        Task<TEntity> GetByEmailAsync<TEntity>(string email) where TEntity : class, new();

        Task<ListUsersViewModel> GetAllAsync(UsersRequestDataModel requestModel);

        Task<Result> UpdateAsync<TEntity>(Expression<Func<ApplicationUser, bool>> select, TEntity model);

        Task<bool> IsUserWithRoleExistAsync(string userEmail, string role);

        Task AddToRoleAsync(ApplicationUser user, string roleName);

        Task RemoveFromRoleAsync(ApplicationUser user, string roleName);
    }
}
