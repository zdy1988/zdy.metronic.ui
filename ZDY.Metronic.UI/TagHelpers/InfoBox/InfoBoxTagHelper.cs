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
    [HtmlTargetElement("info-box", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class InfoBoxTagHelper : HelperBase<Flat2Icon>
    {
        public virtual string Title { get; set; } = "Info Box";

        public virtual string SubTitle { get; set; }

        public virtual string Badge { get; set; }

        public virtual State State { get; set; } = State.None;

        public virtual string Content { get; set; }

        public virtual string NavigationUrl { get; set; }

        public virtual string NavigationText { get; set; } = "Read More";

        public virtual bool IsCleaned { get; set; } = false;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-infobox", true),
                    new CssClass($"kt-infobox--{State.ToValue()}", State.IsUsed()),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var infoBox = BuildInfoBox(childContent);

            if (IsCleaned)
            {
                output.TransformOutput(infoBox);
            }
            else
            {
                output.TagName = "div";

                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Add("class", "kt-portlet");

                output.Content.SetHtmlContent($"<div class='kt-portlet__body'>{infoBox.ToHtml()}</div>");
            }
        }

        internal TagBuilder BuildInfoBox(TagHelperContent childContent)
        {
            var infoBox = new TagBuilder("div");

            infoBox.Attributes.Add("id", Id);

            infoBox.Attributes.Add("class", Classes);

            Badge = String.IsNullOrWhiteSpace(Badge) ? "" : $"<div class='kt-infobox__badge'>{Badge}</div>";

            var head = $@"<div class='kt-infobox__header'>
							  <h2 class='kt-infobox__title'>
                                  {Icon.ToIconContent("mr-2")}
                                  {Title}
                              </h2>
							  {Badge}
						  </div>";

            infoBox.InnerHtml.AppendHtml(head);

            SubTitle = String.IsNullOrWhiteSpace(SubTitle) ? "" : $"<h3 class='kt-infobox__subtitle'>{SubTitle}</h3>";

            Content = String.IsNullOrWhiteSpace(Content) ? childContent.GetContent() : Content;

            NavigationUrl = String.IsNullOrWhiteSpace(NavigationUrl) ? "" : $"<br><br><a href='{NavigationUrl}' class='kt-link'>{NavigationText}</a>";

            var body = $@"<div class='kt-infobox__body'>
                             <div class='kt-infobox__section'>
							    {SubTitle}
							    <div class='kt-infobox__content'>
								    {Content}
                                    {NavigationUrl}
							    </div>
						     </div>
                          </div>";

            infoBox.InnerHtml.AppendHtml(body);

            return infoBox;
        }
    }
}
