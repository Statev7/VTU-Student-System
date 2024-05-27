namespace StudentSystem.Services.Data.Features.Students.Services.Implementation
{
    using System;
    using System.Linq.Expressions;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Services.Data.Features.Users.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;
    using StudentSystem.Services.Messaging;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    public class StudentService : BaseService<Student>, IStudentService
    {
        private const int EntitiesPerPage = 9;

        private readonly ICurrentUserService currentUserService;
        private readonly IEmailSender emailSender;
        private readonly IUserService userService;
        private readonly ILogger<StudentService> logger;

        public StudentService(
            IRepository<Student> repository, 
            IMapper mapper, 
            ICurrentUserService currentUserService,
            IEmailSender emailSender,
            IUserService userService,
            ILogger<StudentService> logger)
            : base(repository, mapper)
        {
            this.currentUserService = currentUserService;
            this.emailSender = emailSender;
            this.userService = userService;
            this.logger = logger;
        }

        public async Task<IPageList<TEntity>> GetAllAsync<TEntity>(Expression<Func<Student, bool>> selector, int currentPage)
            where TEntity : class
            => await this.Repository
                .AllAsNoTracking()
                .OrderByDescending(s => s.CreatedOn)
                .Where(selector)
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToPagedAsync(currentPage, EntitiesPerPage);

        public async Task<Guid> GetIdByUserIdAsync(string userId)
            => await this.Repository
                .AllAsNoTracking()
                .Where(x => x.ApplicationUserId.Equals(userId))
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetScheduleAsync<TEntity>(string userId)
            where TEntity : class
            => await this.Repository
                .AllAsNoTracking()
                .Where(s => s.ApplicationUserId.Equals(userId))
                .SelectMany(s => s.Courses)
                    .Where(cs => cs.Course.IsActive)
                    .SelectMany(cs => cs.Course.Lessons)
                        .Where(l => l.StartTime > DateTime.UtcNow)
                        .OrderBy(l => l.StartTime)
                        .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
               .ToListAsync();

        public async Task<IEnumerable<TEntity>> GetCoursesAsync<TEntity>(string userId)
            where TEntity : class
            => await this.Repository
                    .AllAsNoTracking()
                    .Where(s => s.ApplicationUserId.Equals(userId))
                        .SelectMany(s => s.Courses)
                        .Where(cs => cs.Course.IsActive)
                        .OrderBy(cs => cs.Course.Name)
                        .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Result> CreateAsync(BecomeStudentBindingModel bindingModel)
        {
            var currentUserId = this.currentUserService.GetUserId();

            var studentToCreate = this.Mapper.Map<Student>(bindingModel);
            studentToCreate.ApplicationUserId = currentUserId;

            using var transaction = await this.Repository.BeginTransactionAsync();

            try
            {
                await this.Repository.AddAsync(studentToCreate);
                await userService.UpdateAsync(x => x.Id.Equals(currentUserId), bindingModel);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                this.logger.LogError(ex, $"An exception occurred in the ${nameof(this.CreateAsync)} method");

                return ErrorMesage;
            }

            return Result.Success(SuccessfullyAppliedMessage);
        }

        public async Task<Result> ApproveStudentAsync(string email, bool isApproved)
        {
            var student = await this.Repository
                .All()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.User.Email.Equals(email));

            if (student == null)
            {
                return InvalidStudentErrorMessage;
            }

            try
            {
                await this.ProcessStudentApprovalAsync(isApproved, student);

                var emailMessage = isApproved ? StudentApprovedMessage : StudentNotApprovedMessage;

                await this.emailSender.SendEmailAsync(EmailSender, EmailSenderName, email, ApplicationResultSubject, emailMessage);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"An exception occurred in the ${nameof(this.ApproveStudentAsync)} method");

                return ErrorMesage;
            }

            return true;
        }

        public async Task<bool> IsAppliedAlreadyAsync()
            => await this.Repository
                .AllAsNoTracking()
                .AnyAsync(s => s.ApplicationUserId.Equals(this.currentUserService.GetUserId()) && s.IsApplied);

        private async Task ProcessStudentApprovalAsync(bool isApproved, Student student)
        {
            if (isApproved)
            {
                student.IsApproved = true;
                student.IsApplied = false;

                await this.userService.RemoveFromRoleAsync(student.User, GuestRole);
                await this.userService.AddToRoleAsync(student.User, StudentRole);
            }
            else
            {
                student.User.FirstName = default;
                student.User.LastName = default;

                this.Repository.HardDelete(student);
            }

            await this.Repository.SaveChangesAsync();
        }

        public async Task<Result> SetActiveStatusAsync(Guid id, bool isActive)
        {
            var student = await this.Repository.FindAsync(id);

            if (student == null)
            {
                return InvalidStudentErrorMessage;
            }

            if (student.IsActive != isActive) 
            {
                student.IsActive = isActive;

                this.Repository.Update(student);
                await this.Repository.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> IsActiveAsync(string userId)
            => await this.Repository
                .AllAsNoTracking()
                .AnyAsync(s => s.ApplicationUserId.Equals(userId) && s.IsActive);
    }
}
