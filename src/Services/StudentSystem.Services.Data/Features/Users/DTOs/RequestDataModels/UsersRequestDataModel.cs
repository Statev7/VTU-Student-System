namespace StudentSystem.Services.Data.Features.Users.DTOs.RequestDataModels
{
    using StudentSystem.Common.Enums;

    public class UsersRequestDataModel
    {
        public int CurrentPage { get; set; }

        public string SearchTerm { get; set; }

        public Roles Role { get; set; }
    }
}
