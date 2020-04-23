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
    [HtmlTargetElement("tab-pane", ParentTag = "tabs")]
    public class TabPaneTagHelper : HelperBase<Flat2Icon>
    {
        public virtual string Name { get; set; }

        public virtual string Content { get; set; }

        public virtual bool IsDisabled { get; set; } = false;

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<TabsContext, TabsTagHelper>(out TabsContext tabsContext))
            {
                var childContent = await output.GetChildContentAsync();

                var tabPane = new TagBuilder("div");

                tabPane.Attributes.Add("id", Id);

                tabPane.Attributes.Add("class", "tab-pane");

                tabPane.Attributes.Add("role", "tabpanel");

                var content = String.IsNullOrWhiteSpace(Content) ? childContent.GetContent() : Content;

                tabPane.InnerHtml.AppendHtml(content);

                tabsContext.TabPanes.Add(new Tuple<TabPaneTagHelper, IHtmlContent>(this, tabPane));
            }
        }
    }
}
