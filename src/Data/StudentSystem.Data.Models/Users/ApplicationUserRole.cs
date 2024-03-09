namespace StudentSystem.Data.Models.Users
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public ApplicationUser User { get; set; }

        public ApplicationRole Role { get; set; }
    }
}
