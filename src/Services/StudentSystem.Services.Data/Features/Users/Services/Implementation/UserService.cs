namespace StudentSystem.Services.Data.Features.Users.Services.Implementation
{
    using System.Linq.Expressions;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Users.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Users.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Users.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class UserService : BaseService<ApplicationUser>, IUserService
    {
        private const int EntitiesPerPage = 6;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(
            IRepository<ApplicationUser> repository, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager) 
            : base(repository, mapper)
        {
            this.userManager = userManager;
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
            => await this.Repository
                 .All()
                 .Include(u => u.Student)
                 .FirstOrDefaultAsync(x => x.Email.Equals(email));

        public async Task<TEntity> GetByEmailAsync<TEntity>(string email) 
            where TEntity : class, new()
            => await this.Repository
                .AllAsNoTracking()
                .Where(u => u.Email.Equals(email))
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync()
            ?? new TEntity();

        public async Task<ListUsersViewModel> GetAllAsync(UsersRequestDataModel requestModel)
        {
            var roleName = requestModel.Role.GetEnumValue();

            var pageList = await this.Repository
                .AllAsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(requestModel.SearchTerm), x =>
                    x.Email.Contains(requestModel.SearchTerm) ||
                    x.UserName.Contains(requestModel.SearchTerm))
                .WhereIf(!string.IsNullOrEmpty(roleName), x => 
                    x.UserRoles.Any(y => y.Role.Name.Equals(roleName)))
                .ProjectTo<UserViewModel>(this.Mapper.ConfigurationProvider)
                .ToPagedAsync(requestModel.CurrentPage, EntitiesPerPage);

            var model = new ListUsersViewModel() { PageList = pageList, SearchTerm = requestModel.SearchTerm, Role = requestModel.Role };

            return model;
        }

        public async Task<Result> UpdateAsync<TEntity>(Expression<Func<ApplicationUser, bool>> select, TEntity model)
        {
            var userToUpdate = await this.Repository
                .All()
                .SingleOrDefaultAsync(select);

            if (userToUpdate == null)
            {
                return InvalidUserErrorMessage;
            }

            this.Mapper.Map(model, userToUpdate);

            this.Repository.Update(userToUpdate);
            await this.Repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsUserWithRoleExistAsync(string userEmail, string role)
            => await this.userManager.Users.AnyAsync(u => 
                u.Email.Equals(userEmail) && 
                u.UserRoles.Any(x  => x.Role.Name.Equals(role)));

        public async Task AddToRoleAsync(ApplicationUser user, string roleName)
            => await this.userManager.AddToRoleAsync(user, roleName);

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
            => await this.userManager.RemoveFromRoleAsync(user, roleName);
    }
}
