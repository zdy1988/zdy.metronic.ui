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
    [HtmlTargetElement("switch", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SwitchTagHelper : HelperBase
    {
        public virtual string Name { get; set; }

        public virtual Size Size { get; set; } = Size.None;

        public virtual State State { get; set; } = State.None;

        public virtual bool IsIcon { get; set; } = false;

        public virtual bool IsOutline { get; set; } = false;

        public virtual bool IsDisabled { get; set; } = false;

        public virtual bool IsChecked { get; set; } = false;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-switch", true),
                    new CssClass($"kt-switch--{State.ToValue()}", State.IsUsed()),
                    new CssClass($"kt-switch--{Size.ToValue()}", Size.IsUsed()),
                    new CssClass("kt-switch--outline", IsOutline),
                    new CssClass("kt-switch--icon", IsIcon)
                );
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";

            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", Id);

            output.Attributes.Add("class", Classes);

            var input = new TagBuilder("input");

            input.Attributes.Add("type", "checkbox");

            if (IsDisabled)
            {
                input.Attributes.Add("disabled", "disabled");
            }

            if (IsChecked)
            {
                input.Attributes.Add("checked", "checked");
            }

            if (!String.IsNullOrWhiteSpace(Name))
            {
                input.Attributes.Add("name", Name);
            }

            output.Content.SetHtmlContent($@"<label>{input.ToHtml()}<span></span></label>");
        }
    }
}
