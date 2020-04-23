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
    [HtmlTargetElement("alert", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AlertTagHelper : HelperBase<FlatIcon>
    {
        public virtual string Title { get; set; }

        public virtual string Text { get; set; }

        public virtual State State { get; set; } = State.None;

        public virtual bool CanClosed { get; set; } = false;

        public virtual bool IsShow { get; set; } = true;

        protected virtual string StateValue
        {
            get
            {
                return State.IsUsed() ? State.ToValue() : "secondary";
            }
        }

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("alert fade", true),
                    new CssClass($"alert-{StateValue}", true),
                    new CssClass("show", IsShow),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            output.TagName = "div";

            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", Id);

            output.Attributes.Add("class", Classes);

            if (Icon.IsUsed())
            {
                output.Content.AppendHtml($"<div class='alert-icon'>{Icon.ToIconContent()}</div>");
            }

            var textContainer = new TagBuilder("div");

            textContainer.Attributes.Add("class", "alert-text");

            if (!String.IsNullOrWhiteSpace(Text) || !String.IsNullOrWhiteSpace(Title))
            {
                if (!String.IsNullOrWhiteSpace(Title))
                {
                    textContainer.InnerHtml.AppendHtml($"<h4 class='alert-heading'>{Title}</h4>");
                }

                if (!String.IsNullOrWhiteSpace(Text))
                {
                    textContainer.InnerHtml.AppendHtml($"<p class='mb-0'>{Text}</p>");
                }

                output.Content.AppendHtml(textContainer);
            }
            else
            {
                textContainer.InnerHtml.AppendHtml(childContent);

                output.Content.AppendHtml(textContainer);
            }

            if (CanClosed)
            {
                output.Content.AppendHtml($@"<div class='alert-close'>
												<button type='button' class='close' data-dismiss='alert' aria-label='Close'>
													<span aria-hidden='true'><i class='la la-close'></i></span>
												</button>
											 </div>");
            }
        }
    }
}
