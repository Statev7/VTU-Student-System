namespace StudentSystem.Services.Data.Features.Lessons.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Lessons.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface ILessonService
    {
        Task<TEntity?> GetByIdAsync<TEntity>(Guid id)
            where TEntity : class;

        Task<Result> CreateAsync(LessonFormBindingModel model);

        Task<Result> UpdateAsync(Guid id, LessonFormBindingModel model);

        Task<Result> DeleteAsync(Guid id);
    }
}
