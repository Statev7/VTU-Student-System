namespace StudentSystem.Services.Data.Features.Teachers.Services.Implementation
{
    using System.Collections.Generic;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Services.Data.Features.Users.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    public class TeacherService : BaseService<Teacher>, ITeacherService
    {
        private readonly IUserService userService;
        private readonly ILogger<TeacherService> logger;

        public TeacherService(
            IRepository<Teacher> repository, 
            IMapper mapper,
            IUserService userService,
            ILogger<TeacherService> logger) 
            : base(repository, mapper)
        {
            this.userService = userService;
            this.logger = logger;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            => await this.Repository
                .AllAsNoTracking()
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Result> CreateTeacherAsync(string userEmail, BecomeTeacherBindingModel model)
        {
            var user = await this.userService.GetByEmailAsync(userEmail);

            if (user == null)
            {
                return InvalidUserErrorMessage;
            }

            if (user.Teacher != null)
            {
                return AlreadyATeacherErrorMessage;
            }

            using var transaction = await this.Repository.BeginTransactionAsync();

            try
            {
                var teacher = new Teacher() { AboutМe = model.AboutMe, User = user };

                await this.Repository.AddAsync(teacher);
                await this.userService.AddToRoleAsync(user, TeacherRole);

                if (user.Student == null)
                {
                    await userService.UpdateAsync(x => x.Email.Equals(userEmail), model);

                    await this.userService.RemoveFromRoleAsync(user, GuestRole);
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                this.logger.LogError(ex, $"An exception occurred in the ${nameof(this.CreateTeacherAsync)} method");
            }

            return true;
        }
    }
}
