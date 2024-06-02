namespace StudentSystem.Services.Data.Features.Lessons.Services.Implementation
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Common.Infrastructure.Cache.Services.Contracts;
    using StudentSystem.Common.Infrastructure.Cache.Settings;
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
        private readonly ICacheService cacheService;
        private readonly ICourseService courseService;

        private const string CachePrefix = nameof(Lesson);

        private readonly TimeSpan CacheTimeInHours = TimeSpan.FromHours(8);

        public LessonService(
            IRepository<Lesson> repository,
            IMapper mapper,
            ICacheService cacheService,
            ICourseService courseService) 
            : base(repository, mapper)
        {
            this.cacheService = cacheService;
            this.courseService = courseService;
        }

        #region Public Methods

        public async Task<TEntity?> GetByIdAsync<TEntity>(Guid id)
            where TEntity : class
                => await this.cacheService.GetAsync(
                    CacheKeyGenerator.GenerateKey<TEntity>(id.ToString(), id),
                    async () =>
                    {
                        var lesson = await this.Repository
                            .AllAsNoTracking()
                            .Where(l => l.Id.Equals(id))
                            .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync();

                        return lesson;
                    },
                    CacheTimeInHours);

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

        public async Task<Result> UpdateAsync(Guid id, LessonFormBindingModel model)
        {
            var lessonToUpdate = await this.Repository.FindAsync(id);

            if (lessonToUpdate == null)
            {
                return InvalidLessonErrorMessage;
            }

            var result = await this.ValidateLessonAsync(model);

            if (!result.Succeed)
            {
                return result.Message;
            }

            this.Mapper.Map(model, lessonToUpdate);

            this.Repository.Update(lessonToUpdate);
            await this.Repository.SaveChangesAsync();

            this.cacheService.RemoveByPrefixOrSuffix(id.ToString(), model.CourseId.ToString());

            return Result.Success(SuccessfullyUpdatedMessage);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var lessonToDelete = await this.Repository.FindAsync(id);

            if (lessonToDelete == null) 
            { 
                return InvalidLessonErrorMessage;
            }

            this.Repository.Delete(lessonToDelete);
            await this.Repository.SaveChangesAsync();

            this.cacheService.RemoveByPrefixOrSuffix(id.ToString(), lessonToDelete.CourseId.ToString());

            return Result.Success(SuccessfullyDeletedMessage);
        }

        #endregion

        #region Private Methods

        private async Task<Result<bool>> ValidateLessonAsync(LessonFormBindingModel model)
        {
            var isCourseNotExist = !await this.courseService.IsExistAsync(x => x.Id.Equals(model.CourseId));
            if (isCourseNotExist)
            {
                return Result<bool>.Failure(InvalidCourseErrorMessage);
            }

            var isDatesInvalid = await this.courseService.IsExistAsync(x => 
                x.Id.Equals(model.CourseId) && (model.StartTime < x.StartDate || model.EndTime > x.EndDate));

            if (isDatesInvalid) 
            {
                return Result<bool>.Failure(InvalidDatesErrorMessage);
            }

            return Result<bool>.SuccessWith(true);
        }

        #endregion
    }
}
