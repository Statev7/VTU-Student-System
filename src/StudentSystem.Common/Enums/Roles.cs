namespace StudentSystem.Common.Enums
{
    using StudentSystem.Common.Infrastructure.Attributes;

    using static StudentSystem.Common.Constants.GlobalConstants;

    public enum Roles
    {
        All,
        [CustomizeEnum(AdminRole)]
        Admin,
        [CustomizeEnum(TeacherRole)]
        Teacher,
        [CustomizeEnum(StudentRole)]
        Student,
        [CustomizeEnum(GuestRole)]
        Guest
    }
}
