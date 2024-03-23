namespace StudentSystem.Services.Data.Features.Users.DTOs.ViewModels
{
    using StudentSystem.Common.Enums;
    using StudentSystem.Common.Infrastructure.Collections.Contracts;

    public class ListUsersViewModel
    {
        public IPageList<UserViewModel> PageList { get; set; }

        public string SearchTerm { get; set; } = null!;

        public Roles Role { get; set; }
    }
}
