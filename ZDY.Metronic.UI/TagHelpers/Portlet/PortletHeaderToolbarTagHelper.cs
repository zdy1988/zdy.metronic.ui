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
    [HtmlTargetElement("portlet-header-toolbar", ParentTag = "portlet")]
    public class PortletHeaderToolbarTagHelper : BaseTagHelper
    {
        public virtual bool IsCleaned { get; set; } = true;

        public virtual bool IsColored { get; set; } = false;

        public virtual bool IsPilled { get; set; } = false;

        public virtual bool IsOutline { get; set; } = false;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-portlet__head-toolbar", true),
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

                var toolbar = new TagBuilder("div");

                toolbar.Attributes.Add("id", Id);

                toolbar.Attributes.Add("class", Classes);

                if (childContent.IsEmptyOrWhiteSpace)
                {
                    var tools = $@"	<div class='kt-portlet__head-group'>
								    <a href='javascript:;' data-ktportlet-tool='toggle' class='{BuildToolActionCssClasses(State.Success)}'><i class='la la-angle-down'></i></a>
								    <a href='javascript:;' data-ktportlet-tool='reload' class='{BuildToolActionCssClasses(State.Warning)}'><i class='la la-refresh'></i></a>
								    <a href='javascript:;' data-ktportlet-tool='remove' class='{BuildToolActionCssClasses(State.Danger)}'><i class='la la-close'></i></a>
							    </div>";

                    toolbar.InnerHtml.AppendHtml(tools);
                }
                else
                {
                    toolbar.InnerHtml.AppendHtml(childContent);
                }

                portletContext.HanderToolbar = toolbar;
            }
        }

        internal string BuildToolActionCssClasses(State state)
        {
            var classes = CssClassBuilder.Build(
                 new CssClass("btn btn-sm btn-icon btn-icon-md", true),
                 new CssClass("btn-clean", IsCleaned),
                 new CssClass(IsOutline ? $"btn-outline-{state.ToValue()}" : $"btn-{state.ToValue()}", IsColored),
                 new CssClass("btn-pill", IsPilled));

            return classes;
        }
    }
}
