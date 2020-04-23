using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ZDY.Metronic.UI
{
    internal static class HelperOutputExtension
    {
        internal static void TransformOutput(this TagHelperOutput output, TagBuilder builder)
        {
            output.TagName = builder.TagName;

            output.TagMode = TagMode.StartTagAndEndTag;

            foreach (var attr in builder.Attributes)
            {
                output.Attributes.Add(attr.Key, attr.Value);
            }

            output.Content.SetHtmlContent(builder.InnerHtml);
        }
    }
}
