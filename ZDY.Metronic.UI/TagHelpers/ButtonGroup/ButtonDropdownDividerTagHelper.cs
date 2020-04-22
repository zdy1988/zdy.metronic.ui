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
    [HtmlTargetElement("button-dropdown-divider", ParentTag = "button-dropdown", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ButtonDropdownDividerTagHelper : BaseTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<ButtonDropdownContext, ButtonDropdownTagHelper>(out ButtonDropdownContext buttonDropdownContext))
            {
                var divider = new TagBuilder("div");

                divider.Attributes.Add("id", Id);

                divider.Attributes.Add("class", "dropdown-divider");

                buttonDropdownContext.ButtonOrDividers.Add(divider);
            }
        }
    }
}
