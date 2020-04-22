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
    [HtmlTargetElement("portlet-footer-action-container", ParentTag = "portlet")]
    public class PortletFooterActionContainerTagHelper : BaseTagHelper
    {
        public virtual Size Size { get; set; } = Size.None;

        public virtual Align Align { get; set; } = Align.None;

        public virtual bool IsSoild { get; set; } = false;

        public virtual bool IsFit { get; set; } = false;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-portlet__foot", true),
                    new CssClass($"kt-portlet__foot--{Size.ToValue()}", Size.IsUsed()),
                    new CssClass("kt-portlet__foot--solid", IsSoild),
                    new CssClass("kt-portlet__foot--fit", IsFit),
                    new AlignClass(Align),
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

                portletContext.FooterActionContainer = actionContainer;
            }
        }
    }
}
