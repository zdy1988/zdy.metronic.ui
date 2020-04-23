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
    [HtmlTargetElement("media-group")]
    public class MediaGroupTagHelper : HelperBase
    {
        public virtual int Limit { get; set; }

        public virtual int Total { get; set; }

        public virtual Size Size { get; set; } = Size.None;

        public virtual bool IsCircled { get; set; } = false;

        protected virtual string MediaClasses
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-media--circle", IsCircled),
                    new CssClass($"kt-media--{Size.ToValue()}", Size.IsUsed())
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<MediaGroupContext, MediaGroupTagHelper>(out MediaGroupContext mediaGroupContext))
            {
                await output.GetChildContentAsync();

                if (mediaGroupContext.Medias.Any())
                {
                    output.TagName = "div";

                    output.TagMode = TagMode.StartTagAndEndTag;

                    output.Attributes.Add("id", Id);

                    output.Attributes.Add("class", "kt-media-group");

                    if (Total == 0)
                    {
                        Total = mediaGroupContext.Medias.Count;
                    }

                    if (Limit > mediaGroupContext.Medias.Count)
                    {
                        Limit = mediaGroupContext.Medias.Count;
                    }

                    for (var i = 0; i < mediaGroupContext.Medias.Count; i++)
                    {
                        if (Limit > 0 && Limit <= i)
                        {
                            var ellipsisNumber = Total - Limit;

                            if (ellipsisNumber > 0)
                            {
                                output.Content.AppendHtml(BuildEllipsis(ellipsisNumber));
                            }

                            break;
                        }

                        var media = (TagBuilder)mediaGroupContext.Medias[i];

                        media.AddCssClass(MediaClasses);

                        output.Content.AppendHtml(media);
                    }

                    return;
                }
            }

            output.SuppressOutput();
        }

        internal string BuildEllipsis(int ellipsisNumber)
        {
            return $"<a href='#' class='kt-media {MediaClasses}'><span>{ellipsisNumber}+</span></a>";
        }
    }
}
