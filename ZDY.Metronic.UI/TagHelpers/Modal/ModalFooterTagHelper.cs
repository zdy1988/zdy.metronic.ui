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
    [HtmlTargetElement("modal-footer", ParentTag = "modal")]
    public class ModalFooterTagHelper : HelperBase
    {
        public virtual bool IsCloseDestroyed { get; set; } = false;

        public virtual string CloseText { get; set; } = "Close";

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<ModalContext, ModalTagHelper>(out ModalContext modalContext))
            {
                var childContent = await output.GetChildContentAsync();

                var footer = new TagBuilder("div");

                footer.AddCssClass("modal-footer");

                if (!IsCloseDestroyed)
                {
                    footer.InnerHtml.AppendHtml($"<button type='button' class='btn btn-secondary' data-dismiss='modal'>{CloseText}</button>");
                }

                if (!childContent.IsEmptyOrWhiteSpace)
                {
                    footer.InnerHtml.AppendHtml(childContent);
                }

                modalContext.ModalFooter = footer;
            }
        }
    }
}
