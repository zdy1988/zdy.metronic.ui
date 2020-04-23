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
    [HtmlTargetElement("separator", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SeparatorTagHelper : HelperBase
    {
        public virtual Size Space { get; set; } = Size.None;

        public virtual State State { get; set; } = State.None;

        public virtual bool IsBorderSolid { get; set; } = false;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-separator", true),
                    new CssClass("kt-separator--dashed", !IsBorderSolid),
                    new CssClass($"kt-separator--{State.ToValue()}", State.IsUsed()),
                    new CssClass($"kt-separator--space-{Space.ToValue()}", Space.IsUsed()),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", Id);

            output.Attributes.Add("class", Classes);
        }
    }
}
