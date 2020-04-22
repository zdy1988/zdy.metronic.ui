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
    [HtmlTargetElement(Attributes = "modal-target")]
    public class ModalTargetTagHelper : TagHelper
    {
        public virtual string ModalTarget { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!String.IsNullOrWhiteSpace(ModalTarget))
            {
                output.Attributes.RemoveAll("modal-target");

                output.Attributes.Add("data-toggle", "modal");

                output.Attributes.Add("data-target", $"#{ModalTarget}");
            }
        }
    }
}
