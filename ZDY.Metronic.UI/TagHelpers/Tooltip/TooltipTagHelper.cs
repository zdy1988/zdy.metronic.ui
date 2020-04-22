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
    [HtmlTargetElement(Attributes = "tooltip")]
    [HtmlTargetElement(Attributes = "tooltip-skin")]
    [HtmlTargetElement(Attributes = "tooltip-placement")]
    [HtmlTargetElement(Attributes = "tooltip-trigger")]
    [HtmlTargetElement(Attributes = "tooltip-offset")]
    public class TooltipTagHelper : TagHelper
    {
        public virtual string Tooltip { get; set; }

        public virtual State TooltipState { get; set; }

        public virtual Placement TooltipPlacement { get; set; } = Placement.None;

        public virtual Trigger TooltipTrigger { get; set; } = Trigger.Hover;

        public virtual ValueTuple<int, int> TooltipOffset { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!String.IsNullOrWhiteSpace(Tooltip))
            {
                output.Attributes.RemoveAll("tooltip");

                output.Attributes.Add("data-toggle", "kt-tooltip");

                output.Attributes.Add("data-original-title", Tooltip);

                output.Attributes.Add("data-container", "body");

                output.Attributes.Add("data-html", "true");

                if (TooltipState.IsUsed())
                {
                    output.Attributes.RemoveAll("tooltip-skin");

                    output.Attributes.Add("data-skin", TooltipState.ToValue());
                }

                if (TooltipPlacement.IsUsed())
                {
                    output.Attributes.RemoveAll("tooltip-placement");

                    output.Attributes.Add("data-placement", TooltipPlacement.ToValue());
                }

                if (TooltipTrigger.IsUsed())
                {
                    output.Attributes.RemoveAll("tooltip-trigger");

                    output.Attributes.Add("data-trigger", TooltipTrigger.ToValue());
                }

                if (TooltipOffset != default)
                {
                    output.Attributes.RemoveAll("tooltip-offset");

                    output.Attributes.Add("data-offset", $"{TooltipOffset.Item1}px {TooltipOffset.Item2}px");
                }
            }
        }
    }
}
