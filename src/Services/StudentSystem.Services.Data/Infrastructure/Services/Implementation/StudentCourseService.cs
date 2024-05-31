namespace StudentSystem.Services.Data.Infrastructure.Services.Implementation
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;

    public class StudentCourseService : BaseService<CourseStudentMap>, IStudentCourseService
    {
        private readonly IStudentService studentService;
        private readonly ICurrentUserService currentUserService;

        public StudentCourseService(
            IRepository<CourseStudentMap> repository,
            IMapper mapper,
            IStudentService studentService,
            ICurrentUserService currentUserService)
            : base(repository, mapper)
        {
            this.studentService = studentService;
            this.currentUserService = currentUserService;
        }

        public async Task AddStudentToCourseAsync(Guid courseId)
        {
            var userId = this.currentUserService.GetUserId();
            var studentId = await this.studentService.GetIdByUserIdAsync(userId);

            var courseStudentMaping = new CourseStudentMap()
            {
                CourseId = courseId,
                StudentId = studentId,
            };

            await this.Repository.AddAsync(courseStudentMaping);
            await this.Repository.SaveChangesAsync();
        }

        public async Task<bool> IsUserRegisteredInCourseAsync(Guid courseId, string userId)
            => await this.Repository
                .AllAsNoTracking()
                .AnyAsync(sc => sc.Student.ApplicationUserId.Equals(userId) && sc.CourseId.Equals(courseId));
    }
}
