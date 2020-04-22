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
    [HtmlTargetElement("media", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MediaTagHelper : BaseTagHelper<Flat2Icon>
    {
        public virtual string Placeholder { get; set; } = "M";

        public virtual string ImageUrl { get; set; }

        public virtual string NavigationUrl { get; set; } = "javascript:;";

        public virtual State State { get; set; } = State.None;

        public virtual Size Size { get; set; } = Size.None;

        public virtual bool IsCircled { get; set; } = false;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-media", true),
                    new CssClass("kt-media--circle", IsCircled),
                    new CssClass($"kt-media--{Size.ToValue()}", Size.IsUsed()),
                    new CssClass($"kt-media--{State.ToValue()}", State.IsUsed() && State != State.Light),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var media = new TagBuilder("a");

            media.Attributes.Add("id", Id);

            media.Attributes.Add("class", Classes);

            media.Attributes.Add("href", NavigationUrl);

            if (!String.IsNullOrWhiteSpace(ImageUrl))
            {
                media.InnerHtml.AppendHtml($"<img src='{ImageUrl}' alt='{Placeholder}'>");
            }
            else if (Icon.IsUsed())
            {
                media.InnerHtml.AppendHtml($"<span>{Icon.ToIconContent()}</span>");
            }
            else
            {
                media.InnerHtml.AppendHtml($"<span>{Placeholder.SubByteString(2).ToUpper()}</span>");
            }

            if (context.TryGetContext<MediaGroupContext, MediaGroupTagHelper>(out MediaGroupContext mediaGroupContext))
            {
                mediaGroupContext.Medias.Add(media);

                output.SuppressOutput();
            }
            else
            {
                output.TransformOutput(media);
            }
        }
    }
}
