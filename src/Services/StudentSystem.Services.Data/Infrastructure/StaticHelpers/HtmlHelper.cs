namespace StudentSystem.Services.Data.Infrastructure.StaticHelpers
{
    using Ganss.Xss;

    public static class HtmlHelper
    {
        private static readonly HtmlSanitizer htmlSanitizer = new HtmlSanitizer();

        public static string Sanitize(string htmlText)
            => htmlSanitizer.Sanitize(htmlText);
    }
}
