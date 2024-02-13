namespace StudentSystem.Web.Infrastructure.Extensions
{
    using System.Drawing;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;


    public static class HtmlHelperExtensions
    {
        private const string IconsPath = "/images/icons";

        public static IHtmlContent RenderPngIcon(this IHtmlHelper htmlHelper, string iconName, string altText = null)
        {
            altText ??= iconName;

            var fileName = $"{iconName}.png";
            var path = Path.Combine(IconsPath, fileName);

            var imgTag = $"<img class=\"icon\" src=\"{htmlHelper.Encode(path)}\" alt=\"{altText}\" />";

            return new HtmlString(imgTag);
        }
    }
}
