namespace StudentSystem.Common.Constants
{
    public static class MimeTypesConstants
    {
        private static IDictionary<string, string> MimeTypes = new Dictionary<string, string>()
        {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" },
                { ".csv", "text/csv" }
        };

        public static string GetMimeType(string extension)
            => MimeTypes[extension];
    }
}
