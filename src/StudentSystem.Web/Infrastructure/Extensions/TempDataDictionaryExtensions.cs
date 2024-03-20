namespace StudentSystem.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using StudentSystem.Services.Data.Infrastructure;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public static class TempDataDictionaryExtensions
    {
        public static void Add(this ITempDataDictionary tempDataDictionary, Result result)
        {
            var notificationType = result.Succeed ? SuccessNotification : ErrorNotification;

            tempDataDictionary.Add(notificationType, result.Message);
        }
    }
}
