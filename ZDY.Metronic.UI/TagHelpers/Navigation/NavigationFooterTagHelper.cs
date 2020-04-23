using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZDY.Metronic.UI.Untils;

namespace ZDY.Metronic.UI.TagHelpers.Navigation
{
    [HtmlTargetElement("navigation-footer", ParentTag = "navigation")]
    public class NavigationFooterTagHelper : HelperBase
    {
        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<NavigationContext, NavigationTagHelper>(out NavigationContext navigationContext))
            {
                var childContent = await output.GetChildContentAsync();

                if (!childContent.IsEmptyOrWhiteSpace)
                {
                    var navFooter = new TagBuilder("li");

                    navFooter.AddCssClass("kt-nav__foot");

                    navFooter.InnerHtml.AppendHtml(childContent);

                    navigationContext.Footer = navFooter;
                }
            }
        }
    }
}
