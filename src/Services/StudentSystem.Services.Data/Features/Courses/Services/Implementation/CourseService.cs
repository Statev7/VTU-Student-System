﻿namespace StudentSystem.Services.Data.Features.Courses.Services.Implementation
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

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
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class CourseService : BaseService<Course>, ICourseService
    {
        private const int ImagesWitdhInPexels = 400;
        private const int CacheTimeInHours = 8;

        private readonly string ImagesFolder = $"courses/{DateTime.Now:MMMM}-{DateTime.Now:yyyy}";

        private readonly IMemoryCache memoryCache;
        private readonly IImageFileService imageFileService;
        private readonly ILogger<CourseService> logger;

        private readonly string cacheRegionKey;
        private const string cacheCollectionKey = "Courses-Collection";

        public CourseService(
            IRepository<Course> repository,
            IMapper mapper,
            IMemoryCache memoryCache,
            IImageFileService imageFileService,
            ILogger<CourseService> logger)
            : base(repository, mapper)
        {
            this.memoryCache = memoryCache;
            this.imageFileService = imageFileService;
            this.logger = logger;

            this.cacheRegionKey = typeof(Course).Name;
        }

        public async Task<ListCoursesViewModel<TEntity>> GetAllAsync<TEntity>(CoursesRequestDataModel requestData, int entitiesPerPage)
        {
            var pagedCourses = string.IsNullOrWhiteSpace(requestData.SearchTerm)
                ? await this.GetCoursesFromCacheAsync<TEntity>(requestData, entitiesPerPage)
                : await this.GetCoursesAsync<TEntity>(requestData, entitiesPerPage);

            var resultModel = new ListCoursesViewModel<TEntity>() { PageList = pagedCourses, RequestData = requestData };

            return resultModel;
        }

        public async Task<TEntity?> GetByIdAsync<TEntity>(Guid id) 
            where TEntity : class
        {
            var key = CacheKeyGenerator.GenerateKey(this.cacheRegionKey, id, typeof(TEntity));

            var course = await this.memoryCache.GetOrCreateAsync(key, async factory =>
            {
                factory.SetAbsoluteExpiration(TimeSpan.FromHours(CacheTimeInHours));

                return await this.Repository.AllAsNoTracking()
                    .Where(x => x.Id.Equals(id))
                    .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            });

            return course;
        }

        public async Task<Result> CreateAsync(CourseFormBindingModel bindingModel)
        {
            var courseToCreate = this.Mapper.Map<Course>(bindingModel);

            try
            {
                courseToCreate.ImageFolder = await this.imageFileService.CreateToFileSystemAsync(bindingModel.Image, ImagesFolder, ImagesWitdhInPexels);

                await this.Repository.AddAsync(courseToCreate);
                await this.Repository.SaveChangesAsync();

                this.memoryCache.ClearRegionFromCache(cacheCollectionKey);
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

            return Result.Success(SuccessfullyCreatedCourseMessage);
        }

        public async Task<Result> UpdateAsync(Guid id, CourseFormBindingModel bindingModel)
        {
            var courseToUpdate = await this.Repository.FindAsync(id);

            if (courseToUpdate == null)
            {
                return InvalidCourseErrorMessage;
            }

            this.Mapper.Map(bindingModel, courseToUpdate);

            this.Repository.Update(courseToUpdate);
            await this.Repository.SaveChangesAsync();

            this.memoryCache.ClearRegionFromCache(CacheKeyGenerator.GenerateRegionKey(this.cacheRegionKey, id), cacheCollectionKey);

            return Result.Success(SuccessfillyUpdatedCourseMessage);
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

            this.memoryCache.ClearRegionFromCache(CacheKeyGenerator.GenerateRegionKey(this.cacheRegionKey, id), cacheCollectionKey);

            return Result.Success(SuccessfillyDeletedCourseMessage);
        }

        public async Task<bool> IsExistAsync(Guid id)
            => await this.Repository
                .AllAsNoTracking()
                .AnyAsync(x => x.Id.Equals(id));

        #region Private Methods

        private async Task<IPageList<TEntity>> GetCoursesFromCacheAsync<TEntity>(CoursesRequestDataModel requestData, int entitiesPerPage)
        {
            var key = CacheKeyGenerator.GenerateKey(cacheCollectionKey, typeof(TEntity), new CacheParameter[]
            {
                new (nameof(requestData.CurrentPage), requestData.CurrentPage),
                new (nameof(requestData.OrderBy).ToString(), requestData.OrderBy.GetEnumValue()),
            });

            var courses = await this.memoryCache.GetOrCreateAsync(key, async factory =>
            {
                factory.SetAbsoluteExpiration(TimeSpan.FromHours(CacheTimeInHours));

                return await this.GetCoursesAsync<TEntity>(requestData, entitiesPerPage);
            });

            return courses;
        }

        private async Task<IPageList<TEntity>> GetCoursesAsync<TEntity>(CoursesRequestDataModel requestData, int entitiesPerPage)
            => await this.Repository
                .AllAsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(requestData.SearchTerm), x => x.Name.Contains(requestData.SearchTerm))
                .OrderBy(requestData.OrderBy.GetEnumValue())
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToPagedAsync(requestData.CurrentPage, entitiesPerPage);

        #endregion
    }
}
