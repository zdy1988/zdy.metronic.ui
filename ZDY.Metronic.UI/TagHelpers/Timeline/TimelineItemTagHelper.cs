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
    [HtmlTargetElement("timeline-item", ParentTag = "timeline", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TimelineItemTagHelper : HelperBase
    {
        public virtual DateTime Time { get; set; } = default;

        public virtual State State { get; set; } = State.Brand;

        public virtual string Content { get; set; }

        protected virtual string StateValue
        {
            get
            {
                return State.IsUsed() && State != State.Light ? State.ToValue() : "brand";
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<TimelineContext, TimelineTagHelper>(out TimelineContext timelineContext))
            {
                var childContent = await output.GetChildContentAsync();

                Content = String.IsNullOrWhiteSpace(Content) ? childContent.GetContent() : Content;

                if (timelineContext.Version == TimelineVersion.V1)
                {
                    timelineContext.Items.Add(BuildV1Item());
                }
                else if (timelineContext.Version == TimelineVersion.V2)
                {
                    timelineContext.Items.Add(BuildV2Item());
                }
                else if (timelineContext.Version == TimelineVersion.V3)
                {
                    timelineContext.Items.Add(BuildV3Item());
                }
            }
        }

        internal TagBuilder BuildV1Item()
        {
            var item = new TagBuilder("div");

            item.AddCssClass("kt-timeline-v1__item ");

            item.InnerHtml.AppendHtml($"<div class='kt-timeline-v1__item-circle'><div class='kt-bg-{StateValue}'></div></div>");

            item.InnerHtml.AppendHtml($"<span class='kt-timeline-v1__item-time kt-font-{StateValue}'>{GetTime()}</span>");

            item.InnerHtml.AppendHtml($"<div class='kt-timeline-v1__item-content'>{Content}</div>");

            return item;
        }

        internal TagBuilder BuildV2Item()
        {
            var item = new TagBuilder("div");

            item.AddCssClass($"kt-timeline-v2__item");

            item.InnerHtml.AppendHtml($"<span class='kt-timeline-v2__item-time'>{GetTime()}</span>");

            item.InnerHtml.AppendHtml($"<div class='kt-timeline-v2__item-cricle'><i class='fa fa-genderless kt-font-{StateValue}'></i></div>");

            item.InnerHtml.AppendHtml($"<div class='kt-timeline-v2__item-text'>{Content}</div>");

            return item;
        }

        internal TagBuilder BuildV3Item()
        {
            var item = new TagBuilder("div");

            item.AddCssClass($"kt-timeline-v3__item kt-timeline-v3__item--{StateValue}");

            item.InnerHtml.AppendHtml($"<span class='kt-timeline-v3__item-time'>{GetTime()}</span>");

            item.InnerHtml.AppendHtml($"<div class='kt-timeline-v3__item-desc' style='min-height: 38.75px;'>{Content}</div>");

            return item;
        }

        internal string GetTime()
        {
            if (Time.Date.Equals(DateTime.Now.Date))
            { 
                
            }

            return Time.ToString("t");
        }
    }
}
