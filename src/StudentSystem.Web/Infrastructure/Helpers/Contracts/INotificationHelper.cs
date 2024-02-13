namespace StudentSystem.Web.Infrastructure.Helpers.Contracts
{
    using StudentSystem.Services.Data.Infrastructure;

    public interface INotificationHelper
    {
        (string notificationType, string message) GenerateNotification(Result result, string successMessage, string errorMessage = null);
    }
}
