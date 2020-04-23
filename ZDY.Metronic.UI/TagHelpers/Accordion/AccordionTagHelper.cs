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
    [HtmlTargetElement("accordion")]
    public class AccordionTagHelper : HelperBase
    {
        public virtual AccordionMode Mode { get; set; } = AccordionMode.None;

        public virtual AccordionToggleIcon ToggleIcon { get; set; } = AccordionToggleIcon.None;

        public virtual bool IsExclusived { get; set; } = true;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("accordion", true),
                    new CssClass($"accordion-{Mode.ToValue()}", Mode.IsUsed()),
                    new CssClass($"accordion-toggle-{ToggleIcon.ToValue()}", ToggleIcon.IsUsed()),
                    new CssClass("accordion-svg-icon", Mode == AccordionMode.Light && ToggleIcon == AccordionToggleIcon.Svg),
                    new CssClass("accordion-panel", Mode == AccordionMode.Solid && ToggleIcon == AccordionToggleIcon.Svg),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<AccordionContext, AccordionTagHelper>(out AccordionContext accordionContext))
            {
                accordionContext.AccordionId = IsExclusived ? Id : "";

                accordionContext.ToggleIcon = ToggleIcon;

                await output.GetChildContentAsync();

                if (accordionContext.AccordionCards.Count > 0)
                {
                    var accordion = new TagBuilder("div");

                    accordion.Attributes.Add("id", Id);

                    accordion.Attributes.Add("class", Classes);

                    foreach (var card in accordionContext.AccordionCards)
                    {
                        accordion.InnerHtml.AppendHtml(card);
                    }

                    output.TransformOutput(accordion);

                    return;
                }
            }

            output.SuppressOutput();
        }
    }
}
