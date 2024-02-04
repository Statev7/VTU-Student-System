namespace StudentSystem.Services.Data.Features.Students.Services.Contracts
{
    using StudentSystem.Services.Data.Common;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;

    public interface IStudentService
    {
        Task<Result> CreateAsync(BecomeStudentBindingModel model);
    }
}
