namespace StudentSystem.Services.Data.Features.Courses.Services.Contracts
{
    using System.Linq.Expressions;

    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface ICourseService
    {
        Task<ListCoursesViewModel<TEntity>> GetAllAsync<TEntity>(
            CoursesRequestDataModel requestData, 
            int entitiesPerPage, 
            bool includeExpiredCourses = false,
            bool includeAlreadyStarted = false)
            where TEntity : class;

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : class;

        Task<TEntity?> GetByIdAsync<TEntity>(Guid id) 
            where TEntity : class;

        Task<Result> CreateAsync(CourseFormBindingModel bindingModel);

        Task<Result> UpdateAsync(Guid id, CourseFormBindingModel bindingModel);

        Task<Result> DeleteAsync(Guid id);

        Task<bool> IsExistAsync(Expression<Func<Course, bool>> selector);
    }
}
