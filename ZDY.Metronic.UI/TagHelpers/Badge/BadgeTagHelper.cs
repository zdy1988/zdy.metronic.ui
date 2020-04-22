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
    [HtmlTargetElement("badge", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class BadgeTagHelper : BaseTagHelper
    {
        public virtual string Text { get; set; }

        public virtual State State { get; set; } = State.None;

        public virtual Size Size { get; set; } = Size.None;

        public virtual BadgeShape Shape { get; set; } = BadgeShape.Circle;

        public virtual BadgeMode Mode { get; set; } = BadgeMode.None;

        public virtual FontWeight FontWeight { get; set; } = FontWeight.None;

        protected virtual string StateValue
        {
            get
            {
                var state = State.IsUsed() ? State.ToValue() : "dark";

                if (Mode == BadgeMode.Unified)
                {
                    return $"unified-{state}";
                }

                return state;
            }
        }

        protected virtual bool IsUsedInline
        {
            get
            {
                return Text.GetByteLength() > 2;
            }
        }

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-badge", true),
                    new CssClass($"kt-badge--{StateValue}", true),
                    new CssClass($"kt-badge--{Size.ToValue()}", Size.IsUsed()),
                    new CssClass($"kt-badge--{Shape.ToValue()}", Shape.IsUsed()),
                    new CssClass($"kt-badge--{Mode.ToValue()}", Mode.IsUsed()),
                    new CssClass($"kt-badge--outline", Mode == BadgeMode.Outline_2x),
                    new CssClass($"kt-badge--{FontWeight.ToValue()}", FontWeight.IsUsed()),
                    new CssClass($"kt-badge--inline", IsUsedInline),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var badge = new TagBuilder("span");

            badge.Attributes.Add("id", Id);

            badge.Attributes.Add("class", Classes);

            badge.InnerHtml.Append(Text);


            if (context.TryGetContext<BadgePlacedContext, BadgeTagHelper>(out BadgePlacedContext badgePlacedContext))
            {
                badgePlacedContext.Badges.Add(badge);

                output.SuppressOutput();
            }
            else
            {
                output.TransformOutput(badge);
            } 
        }
    }
}
