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
    [HtmlTargetElement("tabs")]
    public class TabsTagHelper : BaseTagHelper
    {
        public virtual State State { get; set; } = State.None;

        public virtual Size Space { get; set; } = Size.None;

        public virtual bool IsJustified { get; set; } = false;

        public virtual int ActiveIndex { get; set; } = 0;

        public virtual int Limit { get; set; } = int.MaxValue;

        public virtual string LimitText { get; set; } = "More";

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("tab-list row", true),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        protected virtual string NavClasses
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("nav nav-tabs nav-tabs-line nav-tabs-bold nav-tabs-line-2x", true),
                    new CssClass("nav-fill", IsJustified),
                    new CssClass($"nav-tabs-line-{State.ToValue()}", State.IsUsed()),
                    new CssClass($"nav-tabs-space-{Space.ToValue()}", Space.IsUsed()),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<TabsContext, TabsTagHelper>(out TabsContext tabsContext))
            {
                await output.GetChildContentAsync();

                if (tabsContext.TabPanes.Count > 0)
                {
                    output.TagName = "div";

                    output.TagMode = TagMode.StartTagAndEndTag;

                    output.Attributes.Add("id", Id);

                    output.Attributes.Add("role", "tablist");

                    output.Attributes.Add("class", Classes);

                    var content = $@"<div class='col-12'>
                                         {BuildTabNav(tabsContext).ToHtml()}
                                         {BuildTabContent(tabsContext).ToHtml()} 
                                     </div>";

                    output.Content.SetHtmlContent(content);

                    return;
                }
            }

            output.SuppressOutput();
        }

        internal IHtmlContent BuildTabNav(TabsContext tabsContext)
        {
            var nav = new TagBuilder("ul");

            nav.Attributes.Add("class", NavClasses);

            nav.Attributes.Add("role", "tabnav");

            TagBuilder dropdownMenu = null;

            for (var i = 0; i < tabsContext.TabPanes.Count; i++)
            {
                var tab = tabsContext.TabPanes[i];

                var icon = tab.Item1.Icon.IsUsed() ? tab.Item1.Icon.ToIconContent() : "";

                var disabled = tab.Item1.IsDisabled ? "disabled" : "";

                if (Limit > i)
                {
                    var active = ActiveIndex == i ? "active" : "";

                    nav.InnerHtml.AppendHtml($@"<li class='nav-item'>
											        <a class='nav-link {active} {disabled}' data-toggle='tab' href='#{tab.Item1.Id}'>
                                                        {icon}
                                                        {tab.Item1.Name}
                                                    </a>
											    </li>");
                }
                else
                {
                    if (dropdownMenu == null)
                    {
                        dropdownMenu = new TagBuilder("div");

                        dropdownMenu.Attributes.Add("class", "dropdown-menu");
                    }

                    dropdownMenu.InnerHtml.AppendHtml($@"<a class='dropdown-item {disabled}' data-toggle='tab' href='#{tab.Item1.Id}'>
                                                             {icon}
                                                             {tab.Item1.Name}
                                                         </a>");
                }
            }

            if (dropdownMenu.IsNotNull())
            {
                var active = ActiveIndex >= Limit ? "active" : "";

                var dropdown = $@"<li class='nav-item dropdown'>
								   	<a class='nav-link dropdown-toggle {active}' data-toggle='dropdown' href='javascript:;' role='button' aria-haspopup='true' aria-expanded='false'>{LimitText}</a>
								   	{dropdownMenu.ToHtml()}
								  </li>";

                nav.InnerHtml.AppendHtml(dropdown);
            }

            return nav;
        }

        internal IHtmlContent BuildTabContent(TabsContext tabsContext)
        {
            var content = new TagBuilder("div");

            content.Attributes.Add("class", "tab-content");

            for (var i = 0; i < tabsContext.TabPanes.Count; i++)
            {
                var pane = (TagBuilder)tabsContext.TabPanes[i].Item2;

                var active = ActiveIndex == i ? "active" : "";

                pane.AddCssClass(active);

                content.InnerHtml.AppendHtml(pane);
            }

            return content;
        }
    }
}
