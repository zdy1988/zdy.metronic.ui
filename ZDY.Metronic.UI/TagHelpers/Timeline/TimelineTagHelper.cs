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
    [HtmlTargetElement("timeline")]
    public class TimelineTagHelper : HelperBase
    {
        public virtual TimelineVersion Version { get; set; } = TimelineVersion.V2;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass($"kt-timeline-{Version.ToValue()}", true),
                    new CssClass($"kt-timeline-v1--justified", Version == TimelineVersion.V1),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<TimelineContext, TimelineTagHelper>(out TimelineContext timelineContext))
            {
                timelineContext.Version = Version;

                await output.GetChildContentAsync();

                if (timelineContext.Items.Any())
                {
                    output.TagName = "div";

                    output.TagMode = TagMode.StartTagAndEndTag;

                    output.Attributes.Add("id", Id);

                    output.Attributes.Add("class", Classes);

                    var items = new TagBuilder("div");

                    items.AddCssClass($"kt-timeline-{Version.ToValue()}__items");

                    if (Version == TimelineVersion.V1)
                    {
                        items.InnerHtml.AppendHtml("<div class='kt-timeline-v1__marker'></div>");
                    }

                    foreach (var item in timelineContext.Items)
                    {
                        items.InnerHtml.AppendHtml(item);
                    }

                    output.Content.SetHtmlContent(items);

                    return;
                }
            }

            output.SuppressOutput();
        }
    }
}
