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
    [HtmlTargetElement("portlet-header-action-container", ParentTag = "portlet")]
    public class PortletHeaderActionContainerTagHelper : BaseTagHelper
    {
        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-portlet__head-actions", true),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<PortletContext, PortletTagHelper>(out PortletContext portletContext))
            {
                var childContent = await output.GetChildContentAsync();

                var actionContainer = new TagBuilder("div");

                actionContainer.Attributes.Add("id", Id);

                actionContainer.Attributes.Add("class", Classes);

                actionContainer.InnerHtml.AppendHtml(childContent);

                var toobar = new TagBuilder("div");

                toobar.Attributes.Add("class", "kt-portlet__head-toolbar");

                toobar.InnerHtml.AppendHtml(actionContainer);

                portletContext.HanderActionContainer = toobar;
            }
        }
    }
}
