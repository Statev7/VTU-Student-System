namespace StudentSystem.Services.Data.Features.Teachers.Services.Implementation
{
    using System.Collections.Generic;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Services.Data.Features.Users.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    public class TeacherService : BaseService<Teacher>, ITeacherService
    {
        private const int LessonsPerPage = 6;

        private readonly IUserService userService;
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<TeacherService> logger;

        public TeacherService(
            IRepository<Teacher> repository,
            IMapper mapper,
            IUserService userService,
            ICurrentUserService currentUserService,
            ILogger<TeacherService> logger)
            : base(repository, mapper)
        {
            this.userService = userService;
            this.currentUserService = currentUserService;
            this.logger = logger;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            => await this.Repository
                .AllAsNoTracking()
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<CourseSelectionItemViewModel>> GetMyCoursesAsync() 
            => await this.Repository
                .AllAsNoTracking()
                .Where(x => x.ApplicationUserId.Equals(this.currentUserService.GetUserId()))
                    .SelectMany(x => x.Courses)
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Name)
                .ProjectTo<CourseSelectionItemViewModel>(this.Mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IPageList<TEntity>> GetMyScheduleAsync<TEntity>(int currentPage)
            where TEntity : class
            => await this.Repository
                .AllAsNoTracking()
                .Where(x => x.ApplicationUserId.Equals(this.currentUserService.GetUserId()))
                    .SelectMany(x => x.Courses)
                    .Where(x => x.IsActive)
                        .SelectMany(c => c.Lessons)
                        .Where(x => x.StartTime.Date >= DateTime.UtcNow.Date)
                        .OrderBy(x => x.StartTime)
                        .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToPagedAsync(currentPage, LessonsPerPage);

        public async Task<Guid> GetIdByUserId(string userId)
            => await this.Repository
                .AllAsNoTracking()
                .Where(t => t.ApplicationUserId.Equals(userId))
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

        public async Task<Result> CreateTeacherAsync(string userEmail, BecomeTeacherBindingModel bindingModel)
        {
            var user = await this.userService.GetByEmailAsync(userEmail);

            if (user == null)
            {
                return Result.Failure(InvalidUserErrorMessage);
            }

            if (user.Teacher != null)
            {
                return Result.Failure(AlreadyATeacherErrorMessage);
            }

            using var transaction = await this.Repository.BeginTransactionAsync();

            try
            {
                var teacher = new Teacher() { AboutМe = bindingModel.AboutMe, User = user };

                await this.Repository.AddAsync(teacher);
                await this.userService.AddToRoleAsync(user, TeacherRole);

                if (user.Student == null)
                {
                    await userService.UpdateAsync(x => x.Email.Equals(userEmail), bindingModel);

                    await this.userService.RemoveFromRoleAsync(user, GuestRole);
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                this.logger.LogError(ex, $"An exception occurred in the ${nameof(this.CreateTeacherAsync)} method");
            }

            return Result.Success(SuccessfullyCreateTeacher);
        }

        public async Task<bool> IsLeadTheCourseAsync(string userId, Guid courseId)
            => await this.Repository
                .AllAsNoTracking()
                .AnyAsync(u => u.ApplicationUserId.Equals(userId) && u.Courses
                    .Any(course => course.Id.Equals(courseId)));
    }
}
