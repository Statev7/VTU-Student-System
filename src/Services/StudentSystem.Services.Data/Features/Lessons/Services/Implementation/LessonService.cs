namespace StudentSystem.Services.Data.Features.Lessons.Services.Implementation
{
    using System.Threading.Tasks;

    using AutoMapper;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Lessons.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class LessonService : BaseService<Lesson>, ILessonService
    {
        private readonly ICourseService courseService;

        public LessonService(
            IRepository<Lesson> repository,
            IMapper mapper,
            ICourseService courseService) 
            : base(repository, mapper)
        {
            this.courseService = courseService;
        }

        public async Task<Result> CreateAsync(LessonFormBindingModel model)
        {
            var result = await this.ValidateLessonAsync(model);

            if (!result.Succeed) 
            {
                return result.Message;
            }

            var lessonToCreate = this.Mapper.Map<Lesson>(model);

            await this.Repository.AddAsync(lessonToCreate);
            await this.Repository.SaveChangesAsync();

            return Result.Success(SuccessfullyCreatedMessage);
        }

        private async Task<Result<bool>> ValidateLessonAsync(LessonFormBindingModel model)
        {
            var isCourseNotExist = !await this.courseService.IsExistAsync(x => x.Id.Equals(model.CourseId));
            if (isCourseNotExist)
            {
                return Result<bool>.Failure(InvalidCourseErrorMessage);
            }

            var isDatesInvalid = await this.courseService.IsExistAsync(x => 
                x.Id.Equals(model.CourseId) && 
                model.StartTime < x.StartDate ||
                model.EndTime > x.EndDate);

            if (isDatesInvalid) 
            {
                return Result<bool>.Failure(InvalidDatesErrorMessage);
            }

            return Result<bool>.SuccessWith(true);
        }
    }
}
