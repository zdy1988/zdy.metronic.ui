using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace ZDY.Metronic.UI
{
    internal static class HtmlContentExtension
    {
        internal static string ToHtml(this IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
