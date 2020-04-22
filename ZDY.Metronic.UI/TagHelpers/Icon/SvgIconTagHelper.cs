using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZDY.Metronic.UI.Untils;
using ZDY.Metronic.UI.Icons;

namespace ZDY.Metronic.UI.TagHelpers
{
    [HtmlTargetElement("icon-svg", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SvgIconTagHelper : BaseIconTagHelper<SvgIcon>
    {
        protected override string GetIcon() => Svg.Get(Name);

        protected override string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-svg-icon", true),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "svg";

            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", Id);
            output.Attributes.Add("xmlns:xlink", "http://www.w3.org/1999/xlink");
            output.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
            output.Attributes.Add("version", "1.1");
            output.Attributes.Add("width", "24px");
            output.Attributes.Add("height", "24px");
            output.Attributes.Add("viewBox", "0 0 24 24");
            output.Attributes.Add("class", Classes);

            output.Content.SetHtmlContent(GetIcon());
        }
    }
}
