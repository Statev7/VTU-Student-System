namespace StudentSystem.Common.Enums
{
    using System.ComponentModel;

    using static StudentSystem.Common.Constants.GlobalConstants;

    public enum Roles
    {
        All,
        [Description(AdminRole)]
        Admin,
        [Description(TeacherRole)]
        Teacher,
        [Description(StudentRole)]
        Student,
        [Description(GuestRole)]
        Guest
    }
}
