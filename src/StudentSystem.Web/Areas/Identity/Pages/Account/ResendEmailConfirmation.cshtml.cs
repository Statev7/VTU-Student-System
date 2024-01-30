#nullable disable

namespace StudentSystem.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.Encodings.Web;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;

    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Messaging;

    using static StudentSystem.Common.Constants.GlobalConstants;

    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;

        public ResendEmailConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid email adress.");

                return this.Page();
            }

            var userId = await this.userManager.GetUserIdAsync(user);
            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, token },
                protocol: Request.Scheme);

            var href = HtmlEncoder.Default.Encode(callbackUrl);

            await this.emailSender.SendEmailAsync(EmailSender, EmailSenderName, Input.Email, ConfirmSubject, string.Format(ConfirmMessage, href));

            //TODO: Move this to TempData message!
            this.ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");

            return this.Page();
        }
    }
}
