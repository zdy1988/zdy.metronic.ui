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
    [HtmlTargetElement("modal")]
    public class ModalTagHelper : HelperBase<Flat2Icon>
    {
        public virtual string Title { get; set; } = "Modal Title";

        public virtual Size Size { get; set; } = Size.None;

        public virtual bool IsCentered { get; set; } = false;

        public virtual bool IsBackDrop { get; set; } = true;

        public virtual bool IsScrollabled { get; set; } = false;

        public virtual int ContentHeight { get; set; }

        public virtual int MaxContentHeight { get; set; }

        protected virtual string DialogClasses
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("modal-dialog", true), 
                    new CssClass("modal-dialog-centered", IsCentered),
                    new CssClass("modal-dialog-scrollable", IsScrollabled || ContentHeight > 0 || MaxContentHeight > 0),
                    new CssClass($"modal-{Size.ToValue()}", Size.IsUsed()),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        protected virtual string ContentStyles
        {
            get
            {
                return StyleBuilder.Build(
                    ("height", $"{ContentHeight}px", ContentHeight > 0), 
                    ("max-height", $"{MaxContentHeight}px", MaxContentHeight > 0)
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<ModalContext, ModalTagHelper>(out ModalContext modalContext))
            {
                var childContent = await output.GetChildContentAsync();

                output.TagName = "div";

                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Add("id", Id);

                output.Attributes.Add("class", "modal fade");

                output.Attributes.Add("role", "dialog");

                output.Attributes.Add("aria-hidden", "true");

                output.Attributes.Add("data-backdrop", IsBackDrop.ToString().ToLower());

                var dialog = new TagBuilder("div");

                dialog.AddCssClass("modal-dialog");

                dialog.AddCssClass(DialogClasses);

                dialog.Attributes.Add("role", "document");

                var content = new TagBuilder("div");

                content.AddCssClass("modal-content");

                content.Attributes.Add("style", ContentStyles);

                content.InnerHtml.AppendHtml($@"<div class='modal-header'>
								                   <h5 class='modal-title'>
                                                       {Icon.ToIconContent("mr-2")}
                                                       {Title}
                                                   </h5>
								                   <button type='button' class='close' data-dismiss='modal' aria-label='Close'/>
							                    </div>
		                                        <div class='modal-body'>
							                       {childContent.GetContent()}
							                    </div>");

                if (modalContext.ModalFooter.IsNotNull())
                {
                    content.InnerHtml.AppendHtml(modalContext.ModalFooter);
                }

                dialog.InnerHtml.AppendHtml(content);

                output.Content.SetHtmlContent(dialog);

                return;
            }

            output.SuppressOutput();
        }
    }
}
