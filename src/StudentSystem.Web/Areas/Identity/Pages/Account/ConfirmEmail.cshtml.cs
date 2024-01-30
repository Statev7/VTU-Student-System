#nullable disable
namespace StudentSystem.Web.Areas.Identity.Pages.Account
{
    using System.Text;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;

    using StudentSystem.Data.Models.Users;

    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return this.RedirectToPage("/Index");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{userId}'.");
            }

            if (user.EmailConfirmed)
            {
                return this.RedirectToPage("/Index");
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await this.userManager.ConfirmEmailAsync(user, token);

            this.StatusMessage = result.Succeeded 
                ? "Thank you for confirming your email." 
                : "Error confirming your email.";

            return this.Page();
        }
    }
}
