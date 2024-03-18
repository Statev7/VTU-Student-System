namespace StudentSystem.Services.Data.Features.Teachers.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Teachers.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface ITeacherService
    {
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();

        Task<Result> CreateTeacherAsync(string userEmail, BecomeTeacherBindingModel model);
    }
}
