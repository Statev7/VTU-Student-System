namespace StudentSystem.Services.Data.Features.Lessons.Services.Contracts
{
    using System.Linq.Expressions;

    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface ILessonService
    {
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : class;

        Task<TEntity?> GetByIdAsync<TEntity>(Guid id)
            where TEntity : class;

        Task<Result> CreateAsync(LessonFormBindingModel model);

        Task<Result> UpdateAsync(Guid id, LessonFormBindingModel model);

        Task<Result> DeleteAsync(Guid id);

        Task<bool> IsExistAsync(Expression<Func<Lesson, bool>> selector);
    }
}
