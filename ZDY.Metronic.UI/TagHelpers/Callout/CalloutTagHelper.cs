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
    [HtmlTargetElement("callout", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CalloutTagHelper : HelperBase<Flat2Icon>
    {
        public virtual string Title { get; set; } = "Callout Title";

        public virtual string Content { get; set; }

        public virtual State State { get; set; } = State.None;

        public virtual bool IsDiagonalBackground { get; set; } = false;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-portlet kt-callout", true),
                    new CssClass($"kt-callout--{State.ToValue()}", State.IsUsed()),
                    new CssClass($"kt-callout--diagonal-bg", IsDiagonalBackground),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<CalloutContext, CalloutTagHelper>(out CalloutContext calloutContext))
            {
                var childContent = await output.GetChildContentAsync();

                output.TagName = "div";

                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Add("id", Id);

                output.Attributes.Add("class", Classes);

                Content = String.IsNullOrWhiteSpace(Content) ? childContent.GetContent() : Content;

                var body = new TagBuilder("div");

                body.AddCssClass("kt-callout__body");

                body.InnerHtml.AppendHtml($@"<div class='kt-callout__content'>
							   	 	            <h3 class='kt-callout__title'>{Title}</h3>
							   	 	            <p class='kt-callout__desc'>
							   	 		            {Content}
							   	 	            </p>
							   	             </div>");

                if (calloutContext.CalloutAction.IsNotNull())
                {
                    body.InnerHtml.AppendHtml($@"<div class='kt-callout__action'>
							   	 	                {calloutContext.CalloutAction.ToHtml()}
							   	                 </div>");
                }

                output.Content.SetHtmlContent($"<div class='kt-portlet__body'>{body.ToHtml()}</div>");

                return;
            }

            output.SuppressOutput();
        }
    }
}
