namespace StudentSystem.Services.Data.Features.Courses.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface ICourseService
    {
        Task<ListCoursesViewModel<TEntity>> GetAllAsync<TEntity>(CoursesRequestDataModel requestData);

        Task<TEntity> GetByIdAsync<TEntity>(Guid id);

        Task<Result> CreateAsync(CourseFormBindingModel bindingModel);

        Task<Result> UpdateAsync(Guid id, CourseFormBindingModel bindingModel);

        Task<Result> DeleteAsync(Guid id);
    }
}
