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
    [HtmlTargetElement("accordion-card", ParentTag = "accordion", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AccordionCardTagHelper : HelperBase<Flat2Icon>
    {
        public virtual string Title { get; set; } = "Accordion Title";

        public virtual string Text { get; set; }

        public virtual bool IsActived { get; set; } = false;

        protected virtual string HeaderId
        {
            get
            {
                return $"header-{Id}";
            }
        }

        protected virtual string CollapseId
        {
            get
            {
                return $"collapse-{Id}";
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<AccordionContext, AccordionTagHelper>(out AccordionContext accordionContext))
            {
                var childContent = await output.GetChildContentAsync();

                var card = new TagBuilder("div");

                card.Attributes.Add("id", Id);

                card.Attributes.Add("class", "card");

                var content = String.IsNullOrWhiteSpace(Text) ? childContent.GetContent() : Text;

                var parentId = string.IsNullOrWhiteSpace(accordionContext.AccordionId) ? "" : $"#{accordionContext.AccordionId}";

                var svgToggleIcon = accordionContext.ToggleIcon == AccordionToggleIcon.Svg ? SvgIcon.NavigationAngleDoubleRight.ToIconContent() : "";

                card.InnerHtml.AppendHtml(
                    $@"<div class='card-header' id='{HeaderId}'>
                           <div class='card-title {(IsActived ? "" : "collapsed")}' data-toggle='collapse' data-target='#{CollapseId}' aria-expanded='{IsActived}' aria-controls='{CollapseId}'>
				                {Icon.ToIconContent()}
                                {Title}
                                {svgToggleIcon}
                           </div>
				       </div>
				       <div id='{CollapseId}' class='collapse {(IsActived ? "show" : "")}' aria-labelledby='{HeaderId}' data-parent='{parentId}'>
				           <div class='card-body'>
				               {content}	
				           </div>
				       </div>");

                accordionContext.AccordionCards.Add(card);
            }
        }
    }
}
