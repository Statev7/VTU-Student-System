namespace StudentSystem.Common.Infrastructure
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
