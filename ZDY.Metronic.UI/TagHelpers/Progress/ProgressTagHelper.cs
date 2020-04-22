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
    [HtmlTargetElement("progress",TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ProgressTagHelper  : BaseTagHelper
    {
        public virtual State State { get; set; } = State.None;

        public virtual Size Size { get; set; } = Size.None;

        public virtual int Max { get; set; } = 100;

        public virtual int Min { get; set; } = 0;

        public virtual int Step { get; set; } = 1;

        public virtual int Value { get; set; } = 0;

        public virtual ProgressStriped Striped { get; set; } = ProgressStriped.None;

        protected virtual double Progress
        {
            get
            {
                var len = Max - Min;
                var val = Value - Min;
                var step = Math.Pow(10, Step);
                return Math.Round((100 * step * val) / len) / step;
            }
        }

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("progress", true),
                    new CssClass($"progress-{Size.ToValue()}", Size.IsUsed()),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        protected virtual string BarClasses
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("progress-bar", true),
                    new CssClass($"bg-{State.ToValue()}", State.IsUsed()),
                    new CssClass("progress-bar-striped", Striped.IsUsed()),
                    new CssClass("progress-bar-animated", Striped == ProgressStriped.Animated),
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

            var bar = $@"<div class='{BarClasses}' role='progressbar' style='width: {Progress}%' aria-valuenow='{Progress}' aria-valuesetp='{Step}' aria-valuemin='{Min}' aria-valuemax='{Max}'></div>";

            output.Content.SetHtmlContent(bar);
        }
    }
}
