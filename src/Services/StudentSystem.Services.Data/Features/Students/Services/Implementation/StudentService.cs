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

        private readonly string currentUserId;
        private readonly IEmailSender emailSender;
        private readonly ILogger<StudentService> logger;
        private readonly IUserService userService;

        public StudentService(
            IRepository<Student> repository, 
            IMapper mapper, 
            ICurrentUserService currentUserService,
            IEmailSender emailSender,
            ILogger<StudentService> logger,
            IUserService userService)
            : base(repository, mapper)
        {
            this.currentUserId = currentUserService.GetUserId();
            this.emailSender = emailSender;
            this.logger = logger;
            this.userService = userService;
        }

        public async Task<IPageList<TEntity>> GetAllAsync<TEntity>(Expression<Func<Student, bool>> selector, int currentPage)
            => await this.Repository
                .AllAsNoTracking()
                .OrderByDescending(s => s.CreatedOn)
                .Where(selector)
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToPagedAsync(currentPage, EntitiesPerPage);

        public async Task CreateAsync(BecomeStudentBindingModel model)
        {
            var studentToCreate = this.Mapper.Map<Student>(model);
            studentToCreate.ApplicationUserId = this.currentUserId;

            using var transaction = await this.Repository.BeginTransactionAsync();

            try
            {
                await this.Repository.AddAsync(studentToCreate);
                await userService.UpdateAsync(x => x.Id.Equals(this.currentUserId), model);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                this.logger.LogError(ex, $"An exception occurred in the ${nameof(this.CreateAsync)} method");
            }
        }

        public async Task<Result> ApproveStudentAsync(string email, bool isApproved)
        {
            var student = await this.Repository
                .All()
                .Include(s => s.User)
                .SingleOrDefaultAsync(s => s.User.Email == email);

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
                .AnyAsync(s => s.ApplicationUserId.Equals(this.currentUserId) && s.IsApplied);

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

                this.Repository.Delete(student);
            }

            await this.Repository.SaveChangesAsync();
        }
    }
}
