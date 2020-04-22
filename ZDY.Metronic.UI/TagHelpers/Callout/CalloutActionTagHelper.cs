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
    [HtmlTargetElement("callout-action", ParentTag = "callout", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CalloutActionTagHelper : ButtonTagHelper
    {
        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<CalloutContext, CalloutTagHelper>(out CalloutContext calloutContext))
            {
                var childContent = await output.GetChildContentAsync();

                var action = BuildButton(childContent);

                calloutContext.CalloutAction = action;
            }
        }
    }
}
