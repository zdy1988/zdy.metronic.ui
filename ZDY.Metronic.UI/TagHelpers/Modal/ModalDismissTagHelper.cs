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
    [HtmlTargetElement(Attributes = "modal-dismiss")]
    public class ModalDismissTagHelper : TagHelper
    {
        public virtual bool ModalDismiss { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ModalDismiss)
            {
                output.Attributes.RemoveAll("modal-dismiss");

                output.Attributes.Add("data-dismiss", "modal");
            }
        }
    }
}
