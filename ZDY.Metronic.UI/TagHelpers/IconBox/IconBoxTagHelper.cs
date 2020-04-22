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
    [HtmlTargetElement("icon-box", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class IconBoxTagHelper : BaseTagHelper<SvgIcon>
    {
        public virtual string Title { get; set; } = "Icon Box";

        public virtual string Content { get; set; }

        public virtual State State { get; set; } = State.None;

        public virtual IconBoxWave Wave { get; set; } = IconBoxWave.None;

        public virtual string NavigationUrl { get; set; } = "javascript:;";

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-portlet kt-iconbox", true),
                    new CssClass($"kt-iconbox--{State.ToValue()}", State.IsUsed()),
                    new CssClass($"kt-iconbox--{Wave.ToValue()}", Wave.IsUsed()),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            output.TagName = "div";

            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", Id);

            output.Attributes.Add("class", Classes);

            Content = String.IsNullOrWhiteSpace(Content) ? childContent.GetContent() : Content;

            var icon = $"<div class='kt-iconbox__icon'>{(Icon.IsUsed() ? Icon.ToIconContent() : SvgIcon.Compiling.ToIconContent())}</div>";

            var body = $@"<div class='kt-iconbox__body'>
						 	{icon}
						 	<div class='kt-iconbox__desc'>
						 		<h3 class='kt-iconbox__title'>
						 			<a class='kt-link' href='{NavigationUrl}'>{Title}</a>
						 		</h3>
						 		<div class='kt-iconbox__content'>
						 			{Content}
						 		</div>
						 	</div>
						 </div>";

            output.Content.SetHtmlContent($"<div class='kt-portlet__body'>{body}</div>");
        }
    }
}
