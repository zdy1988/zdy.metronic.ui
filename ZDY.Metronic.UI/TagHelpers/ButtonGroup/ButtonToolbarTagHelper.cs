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
    [HtmlTargetElement("button-toolbar")]
    public class ButtonToolbarTagHelper : BaseTagHelper
    {
        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<ButtonToolbarContext, ButtonToolbarTagHelper>(out ButtonToolbarContext buttonToolbarContext))
            {
                await output.GetChildContentAsync();

                if (buttonToolbarContext.Groups.Any())
                {
                    output.TagName = "div";

                    output.TagMode = TagMode.StartTagAndEndTag;

                    output.Attributes.Add("id", Id);

                    output.Attributes.Add("class", $"btn-toolbar {ClassNames}");

                    output.Attributes.Add("role", "toolbar");

                    foreach (var group in buttonToolbarContext.Groups)
                    {
                        if (!buttonToolbarContext.Groups.First().Equals(group))
                        {
                            output.Content.AppendHtml("&nbsp;&nbsp;");
                        }

                        output.Content.AppendHtml(group.ToHtml());
                    }

                    return;
                }
            }

            output.SuppressOutput();
        }
    }
}
