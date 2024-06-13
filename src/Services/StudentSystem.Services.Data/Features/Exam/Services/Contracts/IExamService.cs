namespace StudentSystem.Services.Data.Features.Exam.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Exam.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface IExamService
    {
        Task<Result> CreateAsync(ExamBindingModel model);
    }
}
