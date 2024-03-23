namespace StudentSystem.Services.Data.Features.Courses.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface ICourseService
    {
        Task<TEntity> GetByIdAsync<TEntity>(Guid id);

        Task<Result> CreateAsync(CourseFormBidningModel model);

        Task<Result> UpdateAsync(Guid id, CourseFormBidningModel model);

        Task<Result> DeleteAsync(Guid id);
    }
}
