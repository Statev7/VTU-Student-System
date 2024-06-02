namespace StudentSystem.Common.Infrastructure.Extensions
{
    using System.Security.Claims;

    using static StudentSystem.Common.Constants.GlobalConstants;

    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.IsInRole(AdminRole);

        public static bool IsTeacher(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.IsInRole(TeacherRole);

        public static bool IsStudent(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.IsInRole(StudentRole);
    }
}
