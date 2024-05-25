namespace StudentSystem.Web.Infrastructure.Helpers.Contracts
{
    using StudentSystem.Web.Models;

    public interface IHomeHelper
    {
        Task<HomeViewModel> CreateViewModelAsync();
    }
}
