namespace StudentSystem.Services.Data.Features.Courses.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface ICourseService
    {
        Task<Result> CreateAsync(CourseFormBidningModel model);
    }
}
