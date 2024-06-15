namespace StudentSystem.Services.Data.Features.Exam.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Exam.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface IExamService
    {
        Task<TEnity> GetByIdAsync<TEnity>(Guid id)
            where TEnity : class;

        Task<Result> CreateAsync(CreateExamBindingModel model);

        Task<Result> UpdateAsync(Guid id, UpdateExamBindingModel model);
    }
}
