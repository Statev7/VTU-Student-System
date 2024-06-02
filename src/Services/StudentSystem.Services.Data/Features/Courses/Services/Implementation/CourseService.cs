namespace StudentSystem.Services.Data.Features.Courses.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using StudentSystem.Common.Infrastructure.Cache.Services.Contracts;
    using StudentSystem.Common.Infrastructure.Cache.Settings;
    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.ImageFiles.Services.Contracts;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class CourseService : BaseService<Course>, ICourseService
    {
        private const int ImagesWitdhInPexels = 400;
        private const string CachePrefix = "Courses";

        private readonly string ImagesFolder = $"courses/{DateTime.Now:MMMM}-{DateTime.Now:yyyy}";
        private readonly TimeSpan CacheTimeInHours = TimeSpan.FromHours(1);

        private readonly ICacheService cacheService;
        private readonly IImageFileService imageFileService;
        private readonly ILogger<CourseService> logger;

        public CourseService(
            IRepository<Course> repository,
            IMapper mapper,
            ICacheService cacheService,
            IImageFileService imageFileService,
            ILogger<CourseService> logger)
            : base(repository, mapper)
        {
            this.cacheService = cacheService;
            this.imageFileService = imageFileService;
            this.logger = logger;
        }

        public async Task<ListCoursesViewModel<TEntity>> GetAllAsync<TEntity>(
            CoursesRequestDataModel requestData,
            int entitiesPerPage,
            bool includeExpiredCourses = false,
            bool includeAlreadyStarted = false)
            where TEntity : class
        {
            var pagedCourses = string.IsNullOrWhiteSpace(requestData.SearchTerm)
                ? await this.GetCoursesFromCache<TEntity>(requestData, entitiesPerPage, includeExpiredCourses, includeAlreadyStarted)
                : await this.GetCoursesAsync<TEntity>(requestData, entitiesPerPage, includeExpiredCourses, includeAlreadyStarted);

            var resultModel = new ListCoursesViewModel<TEntity>() { PageList = pagedCourses, RequestData = requestData };

            return resultModel;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : class
            => await this.cacheService.GetAsync<IEnumerable<TEntity>>(
                CacheKeyGenerator.GenerateKey<TEntity>(CachePrefix),
                async () =>
                {
                    var courses = await this.Repository
                        .AllAsNoTracking()
                        .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                        .ToListAsync();

                    return courses;

                },
                CacheTimeInHours);

        public async Task<TEntity?> GetByIdAsync<TEntity>(Guid id)
            where TEntity : class
            => await this.cacheService.GetAsync<TEntity>(
                CacheKeyGenerator.GenerateKey<TEntity>(CachePrefix, id),
                async () =>
                {
                    var course = await this.Repository
                        .AllAsNoTracking()
                        .Where(x => x.Id.Equals(id))
                        .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();

                    return course;

                },
                CacheTimeInHours);

        public async Task<CourseDetailsViewModel> GetDetailsAsync(Guid id)
        {
            var courseDetails = await this.cacheService.GetAsync<CourseDetailsViewModel>(
                CacheKeyGenerator.GenerateKey<CourseDetailsViewModel>(CachePrefix, id),
                async () =>
                {
                    var course = await this.Repository
                        .AllAsNoTracking()
                        .Where(x => x.Id.Equals(id))
                        .ProjectTo<CourseDetailsViewModel>(this.Mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();

                    course.Lessons = await this.Repository
                        .AllAsNoTracking()
                        .Where(x => x.Id.Equals(id))
                            .SelectMany(x => x.Lessons)
                            .Where(l => !l.IsDeleted)
                            .OrderBy(l => l.StartTime)
                        .ProjectTo<LessonPanelViewModel>(this.Mapper.ConfigurationProvider)
                        .ToListAsync();

                    return course;
                },
                CacheTimeInHours);

            return courseDetails;
        }

        public async Task<Result> CreateAsync(CourseFormBindingModel bindingModel)
        {
            var courseToCreate = this.Mapper.Map<Course>(bindingModel);

            try
            {
                courseToCreate.ImageFolder = await this.imageFileService.CreateToFileSystemAsync(bindingModel.Image, ImagesFolder, ImagesWitdhInPexels);

                await this.Repository.AddAsync(courseToCreate);
                await this.Repository.SaveChangesAsync();

                this.cacheService.RemoveByPrefixOrSuffix(CachePrefix);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(courseToCreate.ImageFolder))
                {
                    this.imageFileService.DeleteFromFileSystem(courseToCreate.ImageFolder);
                }

                this.logger.LogError(ex, $"An exception occurred in the ${nameof(this.CreateAsync)} method");

                return ErrorMesage;
            }

            return Result.Success(SuccessfullyCreatedMessage);
        }

        public async Task<Result> UpdateAsync(Guid id, CourseFormBindingModel bindingModel)
        {
            var courseToUpdate = await this.Repository.FindAsync(id);

            if (courseToUpdate == null)
            {
                return InvalidCourseErrorMessage;
            }

            var oldImageFolder = courseToUpdate.ImageFolder;

            try
            {
                if (bindingModel.UploadNewImage && bindingModel.Image != null)
                {
                    courseToUpdate.ImageFolder = await this.imageFileService.CreateToFileSystemAsync(bindingModel.Image, ImagesFolder, ImagesWitdhInPexels);
                }

                this.Mapper.Map(bindingModel, courseToUpdate);

                this.Repository.Update(courseToUpdate);
                await this.Repository.SaveChangesAsync();

                if (!string.IsNullOrEmpty(oldImageFolder) &&
                    bindingModel.UploadNewImage &&
                    oldImageFolder != courseToUpdate.ImageFolder)
                {
                    this.imageFileService.DeleteFromFileSystem(oldImageFolder);
                }

                this.cacheService.RemoveByPrefixOrSuffix(CachePrefix);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(courseToUpdate.ImageFolder) && courseToUpdate.ImageFolder != oldImageFolder)
                {
                    this.imageFileService.DeleteFromFileSystem(courseToUpdate.ImageFolder);
                }

                this.logger.LogError(ex, $"An exception occurred in the ${nameof(this.UpdateAsync)} method");

                return ErrorMesage;
            }

            return Result.Success(SuccessfullyUpdatedMessage);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var courseToDelete = await this.Repository.FindAsync(id);

            if (courseToDelete == null)
            {
                return InvalidCourseErrorMessage;
            }

            this.Repository.Delete(courseToDelete);
            await this.Repository.SaveChangesAsync();

            this.cacheService.RemoveByPrefixOrSuffix(CachePrefix);

            return Result.Success(SuccessfullyDeletedMessage);
        }

        public async Task<bool> IsExistAsync(Expression<Func<Course, bool>> selector)
            => await this.Repository
                .AllAsNoTracking()
                .AnyAsync(selector);

        public async Task ChangeActivityStatusAsync()
        {
            var courses = await this.Repository
                    .All()
                    .Where(c => c.IsActive && DateTime.UtcNow > c.EndDate)
                    .ToListAsync();

            foreach (var course in courses)
            {
                course.IsActive = false;
            }

            this.Repository.BulkUpdate(courses);
            await this.Repository.SaveChangesAsync();
        }

        #region Private Methods

        private async Task<IPageList<TEntity>> GetCoursesFromCache<TEntity>(
            CoursesRequestDataModel requestData,
            int entitiesPerPage,
            bool includeExpiredCourses,
            bool includeAlreadyStarted = false)
            where TEntity : class
        {
            var key = CacheKeyGenerator.GenerateKey<TEntity>(CachePrefix, new CacheParameter[]
            {
                new (nameof(requestData.CurrentPage), requestData.CurrentPage),
                new (nameof(requestData.OrderBy).ToString(), requestData.OrderBy.GetEnumValue()),
            });

            var courses = await this.cacheService.GetAsync<IPageList<TEntity>>(key, async () =>
            {
                return await this.GetCoursesAsync<TEntity>(requestData, entitiesPerPage, includeExpiredCourses, includeAlreadyStarted);
            },
            CacheTimeInHours);

            return courses;
        }

        private async Task<IPageList<TEntity>> GetCoursesAsync<TEntity>(
            CoursesRequestDataModel requestData,
            int entitiesPerPage,
            bool includeExpiredCourses,
            bool includeAlreadyStarted)
            where TEntity : class
            => await this.Repository
                .AllAsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(requestData.SearchTerm), x => x.Name.Contains(requestData.SearchTerm))
                .WhereIf(includeExpiredCourses == false, x => x.IsActive)
                .WhereIf(includeAlreadyStarted == false, x => x.StartDate > DateTime.UtcNow)
                .OrderBy(requestData.OrderBy.GetEnumValue())
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToPagedAsync(requestData.CurrentPage, entitiesPerPage);

        #endregion
    }
}
