namespace StudentSystem.Services.Data.Features.Lessons.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Lessons.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface ILessonService
    {
        Task<Result> CreateAsync(LessonFormBindingModel model); 
    }
}
