namespace StudentSystem.Services.Data.Features.Students.Services.Implementation
{
    using System;
    using System.Linq.Expressions;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.Collections.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;
    using StudentSystem.Services.Messaging;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    public class StudentService : BaseService<Student>, IStudentService
    {
        private const int EntitiesPerPage = 9;

        private readonly ICurrentUserService currentUserService;
        private readonly IEmailSender emailSender;

        public StudentService(
            IRepository<Student> repository, 
            IMapper mapper, 
            ICurrentUserService currentUserService,
            IEmailSender emailSender)
            : base(repository, mapper)
        {
            this.currentUserService = currentUserService;
            this.emailSender = emailSender;
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

            studentToCreate.ApplicationUserId = this.currentUserService.GetUserId();
            studentToCreate.IsApplied = true;

            await this.Repository.AddAsync(studentToCreate);
            await this.Repository.SaveChangesAsync();
        }

        public async Task<Result> ApproveStudentAsync(string email, bool isApproved)
        {
            var student = await this.Repository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(s => s.User.Email == email);

            if (student == null)
            {
                return InvalidStudentErrorMessage;
            }

            if (isApproved)
            {
                student.IsApproved = true;
                student.IsApplied = false;

                this.Repository.Update(student);
            }
            else
            {
                this.Repository.Delete(student);
            }

            await this.Repository.SaveChangesAsync();

            var emailMessage = isApproved ? StudentApprovedMessage : StudentNotApprovedMessage;

            await this.emailSender.SendEmailAsync(EmailSender, EmailSenderName, email, ApplicationResultSubject, emailMessage);

            return true;
        }

        public async Task<bool> IsAppliedAlreadyAsync()
            => await this.Repository
            .AllAsNoTracking()
            .AnyAsync(s => s.ApplicationUserId == this.currentUserService.GetUserId() && s.IsApplied);
    }
}
