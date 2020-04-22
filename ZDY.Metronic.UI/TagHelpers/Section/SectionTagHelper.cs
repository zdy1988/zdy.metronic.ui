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
    [HtmlTargetElement("section")]
    public class SectionTagHelper : BaseTagHelper
    {
        public virtual string Title { get; set; }

        public virtual string Info { get; set; }

        public virtual SectionMode Mode { get; set; } = SectionMode.None;

        public virtual bool IsFit { get; set; } = false;

        public virtual bool IsFitX { get; set; } = false;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-section", true),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        protected virtual string ContentClasses
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-section__content", true),
                    new CssClass($"kt-section__content--{Mode.ToValue()}", Mode.IsUsed()),
                    new CssClass("kt-section__content--fit", IsFit),
                    new CssClass("kt-section__content--x-fit", IsFitX),
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

            if (!String.IsNullOrWhiteSpace(Title))
            {
                output.Content.AppendHtml($"<h3 class='kt-section__title'>{Title}</h3>");
            }

            if (!String.IsNullOrWhiteSpace(Info))
            {
                output.Content.AppendHtml($"<div class='kt-section__info'>{Info}</div>");
            }

            if (!childContent.IsEmptyOrWhiteSpace)
            {
                output.Content.AppendHtml($"<div class='{ContentClasses}'>{childContent.GetContent()}</div>");
            }
        }
    }
}
