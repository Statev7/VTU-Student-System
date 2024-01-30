namespace StudentSystem.Services.Messaging.Senders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    using StudentSystem.Common;

    public class SendGridEmailSender : IEmailSender
    {
        private const string EmptySubjectAndContentErrorMessage = "Subject and message should be provided.";

        private readonly SendGridClient client;

        public SendGridEmailSender(IOptions<ApplicationSettings> options) 
            => this.client = new SendGridClient(options.Value.SendGridApiKey);

        public async Task SendEmailAsync(
            string from, 
            string fromName, 
            string to, 
            string subject, 
            string htmlContent, 
            IEnumerable<EmailAttachment> attachments = null)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException(EmptySubjectAndContentErrorMessage);
            }

            var fromAddress = new EmailAddress(from, fromName);
            var toAddress = new EmailAddress(to);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);
            if (attachments?.Any() == true)
            {
                foreach (var attachment in attachments)
                {
                    message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
                }
            }

            try
            {
                var response = await this.client.SendEmailAsync(message);

                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Body.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
    }
}
