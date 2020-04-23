using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZDY.Metronic.UI.Untils;

namespace ZDY.Metronic.UI.TagHelpers
{
    [HtmlTargetElement("text-box-addon", ParentTag = "text-box")]
    public class TextBoxAddonTagHelper : HelperBase
    {
        public virtual TextBoxAddonAlign Align { get; set; } = TextBoxAddonAlign.Left;

        public virtual bool IsTextAddon { get; set; } = false;

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<TextBoxContext, TextBoxTagHelper>(out TextBoxContext textBoxContext))
            {
                var childContent = await output.GetChildContentAsync();

                if (IsTextAddon)
                {
                    var span = new TagBuilder("span");

                    span.Attributes.Add("id", Id);

                    span.Attributes.Add("class", "input-group-text");

                    span.InnerHtml.AppendHtml(childContent);

                    Assign(span);
                }
                else
                {
                    Assign(childContent);
                }

                void Assign(IHtmlContent content)
                {
                    if (Align == TextBoxAddonAlign.Left)
                    {
                        textBoxContext.PrependAddons.Add(content);
                    }
                    else
                    {
                        textBoxContext.AppendAddons.Add(content);
                    }
                }
            }
        }
    }
}
