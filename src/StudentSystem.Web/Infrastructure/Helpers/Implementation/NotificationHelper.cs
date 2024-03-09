namespace StudentSystem.Web.Infrastructure.Helpers.Implementation
{
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class NotificationHelper : INotificationHelper
    {
        public (string notificationType, string message) GenerateNotification(Result result, string successMessage, string errorMessage = null)
        {
            errorMessage ??= result.Error;

            var notificationType = result.Succeed ? SuccessNotification : ErrorNotification;
            var message = result.Succeed ? successMessage : errorMessage;

            return (notificationType, message);
        }
    }
}
