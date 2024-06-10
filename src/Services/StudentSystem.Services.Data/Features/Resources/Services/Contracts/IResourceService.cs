namespace StudentSystem.Services.Data.Features.Resources.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Resources.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.DTOs.ServiceModels;

    public interface IResourceService
    {
        Task<Result<FileServiceModel>> LoadResourceAsync(Guid id);

        Task<TEntity?> GetByIdAsync<TEntity>(Guid id)
            where TEntity : class;

        Task<Result> CreateAsync(ResourceBindingModel model);

        Task<Result> UpdateAsync(Guid id, ResourceBindingModel model);

        Task<Result> DeleteAsync(Guid id);
    }
}
