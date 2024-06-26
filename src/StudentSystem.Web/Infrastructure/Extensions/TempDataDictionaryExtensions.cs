﻿namespace StudentSystem.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using StudentSystem.Services.Data.Infrastructure;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public static class TempDataDictionaryExtensions
    {
        public static void Add(this ITempDataDictionary tempDataDictionary, bool success, string message)
        {
            var notificationType = success ? SuccessNotification : ErrorNotification;

            tempDataDictionary.Add(notificationType, message);
        }

        public static void Add(this ITempDataDictionary tempDataDictionary, Result result)
            => tempDataDictionary.Add(result.Succeed, result.Message);
    }
}
