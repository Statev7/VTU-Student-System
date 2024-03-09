namespace StudentSystem.Services.Data.Features.Users.DTOs.ViewModels
{
    using static StudentSystem.Common.Constants.GlobalConstants;

    public class UserViewModel
    {
        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public IEnumerable<string> RolesNames { get; set; } = null!;

        public string FormatedRoles => string.Join(", ", this.RolesNames);

        public bool IsTeacher => this.RolesNames.Any(x => x.Equals(TeacherRole));
    }
}
