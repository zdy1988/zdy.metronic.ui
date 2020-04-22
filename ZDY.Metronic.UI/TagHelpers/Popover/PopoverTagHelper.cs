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
    [HtmlTargetElement(Attributes = "popover")]
    [HtmlTargetElement(Attributes = "popover-placement")]
    [HtmlTargetElement(Attributes = "popover-trigger")]
    [HtmlTargetElement(Attributes = "popover-title")]
    [HtmlTargetElement(Attributes = "popover-content")]
    [HtmlTargetElement(Attributes = "popover-offset")]
    public class PopoverTagHelper : TagHelper
    {
        public virtual bool Popover { get; set; } = false;

        public virtual Placement PopoverPlacement { get; set; } = Placement.None;

        public virtual Trigger PopoverTrigger { get; set; } = Trigger.Hover;

        public virtual string PopoverTitle { get; set; }

        public virtual string PopoverContent { get; set; }

        public virtual ValueTuple<int, int> PopoverOffset { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Popover)
            {
                output.Attributes.RemoveAll("popover");

                output.Attributes.Add("data-toggle", "kt-popover");

                output.Attributes.Add("data-container", "body");

                if (PopoverPlacement.IsUsed())
                {
                    output.Attributes.RemoveAll("popover-placement");

                    output.Attributes.Add("data-placement", PopoverPlacement.ToValue());
                }

                if (PopoverTrigger.IsUsed())
                {
                    output.Attributes.RemoveAll("popover-trigger");

                    output.Attributes.Add("data-trigger", PopoverTrigger.ToValue());
                }

                if (!String.IsNullOrWhiteSpace(PopoverTitle))
                {
                    output.Attributes.RemoveAll("popover-title");

                    output.Attributes.Add("data-original-title", PopoverTitle);
                }

                if (!String.IsNullOrWhiteSpace(PopoverContent))
                {
                    output.Attributes.RemoveAll("popover-content");

                    output.Attributes.Add("data-content", PopoverContent);

                    output.Attributes.Add("data-html", "true");
                }

                if (PopoverOffset != default)
                {
                    output.Attributes.RemoveAll("popover-offset");

                    output.Attributes.Add("data-offset", $"{PopoverOffset.Item1}px {PopoverOffset.Item2}px");
                }
            }
        }
    }
}
