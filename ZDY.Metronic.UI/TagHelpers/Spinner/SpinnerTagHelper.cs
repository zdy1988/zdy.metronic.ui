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
    [HtmlTargetElement(Attributes = "spinner")]
    [HtmlTargetElement(Attributes = "spinner-size")]
    [HtmlTargetElement(Attributes = "spinner-state")]
    [HtmlTargetElement(Attributes = "spinner-shorted")]
    public class SpinnerTagHelper : TagHelper
    {
        public virtual bool Spinner { get; set; } = false;

        public virtual Size SpinnerSize { get; set; } = Size.None;

        public virtual State SpinnerState { get; set; } = State.None;

        public virtual Align SpinnerAlign { get; set; } = Align.None;

        public virtual bool SpinnerShorted { get; set; } = false;

        protected virtual string SpinnerStateValue
        {
            get
            {
                return SpinnerState.IsUsed() ? SpinnerState.ToValue() : "info";
            }
        }

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-spinner", true),
                    new CssClass("kt-spinner--v2", SpinnerShorted),
                    new CssClass($"kt-spinner--{SpinnerStateValue}", true),
                    new CssClass($"kt-spinner--{SpinnerSize.ToValue()}", SpinnerSize.IsUsed()),
                    new CssClass($"kt-spinner--{SpinnerAlign.ToValue()}", SpinnerAlign.IsUsed())
                );
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Spinner)
            {
                output.Attributes.RemoveAll("spinner");

                if (output.Attributes.TryGetAttribute("class", out TagHelperAttribute attribute))
                {
                    output.Attributes.SetAttribute("class", $"{attribute.Value} {Classes}");
                }
                else
                {
                    output.Attributes.Add("class", Classes);
                }
            }
        }
    }
}
