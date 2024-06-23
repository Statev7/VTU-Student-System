namespace StudentSystem.Services.Data.Features.StudentCourses.Services.Implementation
{
    using System.Collections.Generic;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Students.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Services.Data.Features.Teachers.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.StaticHelpers;
    using StudentSystem.Services.Messaging;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Common.Constants.NotificationConstants;

    public class StudentCourseService : BaseService<CourseStudentMap>, IStudentCourseService
    {
        private const int StudentsPerPage = 10;

        private readonly IStudentService studentService;
        private readonly ICurrentUserService currentUserService;
        private readonly ITeacherService teacherService;
        private readonly IEmailSender emailSender;

        public StudentCourseService(
            IRepository<CourseStudentMap> repository,
            IMapper mapper,
            IStudentService studentService,
            ICurrentUserService currentUserService,
            ITeacherService teacherService,
            IEmailSender emailSender)
            : base(repository, mapper)
        {
            this.studentService = studentService;
            this.currentUserService = currentUserService;
            this.teacherService = teacherService;
            this.emailSender = emailSender;
        }

        public async Task<ListStudentsViewModel> GetStudentsByCourseAsync(StudentsInCourseRequestData requestData)
        {
            var teacherId = await this.teacherService.GetIdByUserId(this.currentUserService.GetUserId());

            var pageList = await this.Repository
                        .AllAsNoTracking()
                        .Where(x => x.CourseId.Equals(requestData.CourseId) && x.Course.TeacherId.Equals(teacherId))
                        .WhereIf(!string.IsNullOrEmpty(requestData.SearchTerm), x => x.Student.User.FirstName.Contains(requestData.SearchTerm))
                        .OrderBy(x => x.Student.User.FirstName)
                        .ThenBy(x => x.Student.User.LastName)
                        .ProjectTo<StudentWithGradeViewModel>(this.Mapper.ConfigurationProvider)
                        .ToPagedAsync(requestData.CurrentPage, StudentsPerPage);

            requestData.Courses = await this.teacherService.GetMyCoursesAsync();

            var resultModel =  new ListStudentsViewModel() { StudentsPageList = pageList, RequestData = requestData };

            return resultModel;
        }

        public async Task<TEntity?> GetCourseWithExamDetailsAsync<TEntity>(Guid courseId)
            where TEntity : class
        {
            var studentId = await this.studentService.GetIdByUserIdAsync(this.currentUserService.GetUserId());

            var model = await this.Repository
                .AllAsNoTracking()
                .Where(x => x.CourseId.Equals(courseId) && x.StudentId.Equals(studentId))
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return model;
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

        public async Task<bool> IsExistAsync(Guid studentId, Guid courseId)
            => await this.Repository
                .AllAsNoTracking()
                .AnyAsync(x => x.StudentId.Equals(studentId) && x.CourseId.Equals(courseId));

        public Task<bool> HasGradeAsync(Guid studentId, Guid courseId)
            => this.Repository
                .AllAsNoTracking()
                .Where(x => x.StudentId.Equals(studentId) && x.CourseId.Equals(courseId))
                .AnyAsync(x => x.Exam != null);

        #region Private Methods

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

        #endregion
    }
}
