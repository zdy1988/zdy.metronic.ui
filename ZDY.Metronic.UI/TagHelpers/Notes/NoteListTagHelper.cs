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
    [HtmlTargetElement("note-list")]
    public class NoteListTagHelper :HelperBase
    {
        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<NoteListContext, NoteListTagHelper>(out NoteListContext noteListContext))
            {
                await output.GetChildContentAsync();

                if (noteListContext.Notes.Any())
                {
                    output.TagName = "div";

                    output.TagMode = TagMode.StartTagAndEndTag;

                    output.Attributes.Add("id", Id);

                    output.Attributes.Add("class", "kt-notes");

                    var items = new TagBuilder("div");

                    items.AddCssClass("kt-notes__items");

                    foreach (var item in noteListContext.Notes)

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
