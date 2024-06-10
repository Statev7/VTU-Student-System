namespace StudentSystem.Services.Data.Features.Resources.Services.Implementation
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using StudentSystem.Common.Infrastructure.Cache.Services.Contracts;
    using StudentSystem.Common.Infrastructure.Cache.Settings;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.ServiceModels;
    using StudentSystem.Services.Data.Features.Lessons.Services.Contracts;
    using StudentSystem.Services.Data.Features.Resources.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Resources.DTOs.ServiceModels;
    using StudentSystem.Services.Data.Features.Resources.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.DTOs.ServiceModels;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class ResourceService : BaseService<Resource>, IResourceService
    {
        private readonly IFileService fileService;
        private readonly ILessonService lessonService;
        private readonly ICacheService cacheService;
        private readonly ILogger<ResourceService> logger;

        private readonly TimeSpan CacheTimeInHours = TimeSpan.FromHours(12);

        public ResourceService(
            IRepository<Resource> repository,
            IMapper mapper,
            IFileService fileService,
            ILessonService lessonService,
            ICacheService cacheService,
            ILogger<ResourceService> logger)
            : base(repository, mapper)
        {
            this.fileService = fileService;
            this.lessonService = lessonService;
            this.cacheService = cacheService;
            this.logger = logger;
        }

        public async Task<TEntity?> GetByIdAsync<TEntity>(Guid id)
            where TEntity : class
                => await this.cacheService.GetAsync<TEntity>(
                    CacheKeyGenerator.GenerateKey<TEntity>(id),
                    async () =>
                    {
                        var resource = await this.Repository
                        .AllAsNoTracking()
                        .Where(x => x.Id.Equals(id))
                        .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();

                        return resource;
                    },
                    CacheTimeInHours);


        public async Task<Result> CreateAsync(ResourceBindingModel model)
        {
            var isLessonNotExist = !await this.lessonService.IsExistAsync(x => x.Id.Equals(model.LessonId));

            if (isLessonNotExist)
            {
                return InvalidLessonErrorMessage;
            }

            var fileResult = await this.fileService.UploadFileAsync(model.File, model.Name, await this.GenerateFolderAsync(model.LessonId));

            if (!fileResult.Succeed)
            {
                return Result.Failure(fileResult.Message);
            }

            try
            {
                var resourceToCreate = this.Mapper.Map<Resource>(model);

                resourceToCreate.FolderPath = fileResult.Data;
                resourceToCreate.Extension = Path.GetExtension(model.File.FileName);

                await this.Repository.AddAsync(resourceToCreate);
                await this.Repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.fileService.DeleteFromFileSystem(fileResult.Data);

                this.logger.LogError(ex, $"An exception occurred in the ${nameof(this.CreateAsync)} method");

                return Result.Failure(ErrorMesage);
            }

            return Result.Success(SuccessfullyCreatedMessage);
        }

        public async Task<Result<FileServiceModel>> LoadResourceAsync(Guid id)
        {
            var resource = await this.GetByIdAsync<ResourceDetailsServiceModel>(id);

            if (resource == null)
            {
                return Result<FileServiceModel>.Failure(InvalidResourceErrorMessage);
            }

            var result = this.fileService.GetFile(resource.FolderPath, resource.Name, resource.Extension);

            if (!result.Succeed)
            {
                return Result<FileServiceModel>.Failure(result.Message);
            }

            return result;
        }

        public Task<Result> UpdateAsync(ResourceBindingModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var resourceToDelete = await this.Repository.FindAsync(id);

            if (resourceToDelete == null)
            {
                return Result.Failure(InvalidResourceErrorMessage);
            }

            this.Repository.Delete(resourceToDelete);
            await this.Repository.SaveChangesAsync();

            this.cacheService.RemoveByCollectionKeysPrefixes(new CacheKeyCollection(id.ToString(), resourceToDelete.LessonId.ToString()));

            return Result.Success(SuccessfullyDeletedMessage);
        }

        #region Private Methods

        private async Task<string> GenerateFolderAsync(Guid lessonId)
        {
            var lesson = await this.lessonService.GetByIdAsync<LessonCourseNameServiceModel>(lessonId);

            var folder = $"Resources/{lesson.CourseName}/{lesson.Name}";

            return folder;
        }

        #endregion
    }
}
