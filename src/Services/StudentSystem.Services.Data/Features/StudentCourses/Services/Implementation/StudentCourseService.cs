using StudentSystem.Services.Data.Infrastructure;

namespace StudentSystem.Services.Data.Features.StudentCourses.Services.Implementation
{
    using System.Collections.Generic;
    using System.IO.Compression;

    using AutoMapper;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.StaticHelpers;
    using StudentSystem.Services.Messaging;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    public class StudentCourseService : BaseService<CourseStudentMap>, IStudentCourseService
    {
        private readonly IStudentService studentService;
        private readonly ICurrentUserService currentUserService;
        private readonly IEmailSender emailSender;

        public StudentCourseService(
            IRepository<CourseStudentMap> repository,
            IMapper mapper,
            IStudentService studentService,
            ICurrentUserService currentUserService,
            IEmailSender emailSender)
            : base(repository, mapper)
        {
            this.studentService = studentService;
            this.currentUserService = currentUserService;
            this.emailSender = emailSender;
        }

        public async Task AddStudentToCourseAsync(Guid courseId)
        {
            var userId = currentUserService.GetUserId();
            var studentId = await studentService.GetIdByUserIdAsync(userId);

            var courseStudentMaping = new CourseStudentMap()
            {
                CourseId = courseId,
                StudentId = studentId,
            };

            await Repository.AddAsync(courseStudentMaping);
            await Repository.SaveChangesAsync();
        }

        public async Task<bool> IsUserRegisteredInCourseAsync(Guid courseId, string userId)
            => await Repository
                .AllAsNoTracking()
                .AnyAsync(sc => sc.Student.ApplicationUserId.Equals(userId) && sc.CourseId.Equals(courseId));

        public async Task<Result> SendInformationAsync(EmailInformationBindingModel model)
        {
            var studentsEmails = await Repository
                    .AllAsNoTracking()
                    .Where(x => model.SelectedCourses.Contains(x.CourseId))
                    .Select(x => x.Student.User.Email)
                    .Distinct()
                    .ToListAsync();

            if (!studentsEmails.IsNullOrEmpty())
            {
                var sanitizedMessage = HtmlHelper.Sanitize(model.Content);

                var attachments = !model.Attachments.IsNullOrEmpty() 
                    ? await this.ConvertToEmailAttachmentAsync(model.Attachments)
                    : new List<EmailAttachment>();

                var tasks = studentsEmails.Select(async email =>
                {
                    await emailSender.SendEmailAsync(EmailSender, EmailSenderName, email, model.Subject, model.Content, attachments);
                });

                await Task.WhenAll(tasks);
            }

            return Result.Success(string.Format(EmailNotificationMessage, studentsEmails.Count));
        }

        private async Task<IEnumerable<EmailAttachment>> ConvertToEmailAttachmentAsync(IEnumerable<IFormFile> files)
        {
            var tasks = files.Select(async file =>
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                return new EmailAttachment
                {
                    Content = memoryStream.ToArray(),
                    FileName = file.FileName,
                    MimeType = file.ContentType,
                };
            });

           var attachments = await Task.WhenAll(tasks);

            return attachments;
        }
    }
}
