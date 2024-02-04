namespace StudentSystem.Services.Data.Features.Students.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;

    public interface IStudentService
    {
        Task CreateAsync(BecomeStudentBindingModel model);

        Task<bool> IsAppliedAlreadyAsync();
    }
}
