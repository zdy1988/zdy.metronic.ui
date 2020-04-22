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
    [HtmlTargetElement("navigation", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class NavigationTagHelper : BaseTagHelper
    {
        public virtual string Title { get; set; }

        public virtual Size Space { get; set; } = Size.None;

        public virtual List<NavigationItem> Navs { get; set; } = new List<NavigationItem>();

        public virtual bool IsToggled { get; set; } = false;

        public virtual NavigationBullet Bullet { get; set; } = NavigationBullet.Icon;

        protected virtual string Classes
        {
            get {
                return CssClassBuilder.Build(
                    new CssClass("kt-nav", true),
                    new CssClass("kt-nav--bold", Space == Size.Lg),
                    new CssClass($"kt-nav--{Space.ToValue()}-space", Space.IsUsed()),
                    new CssClass($"kt-nav--{Space.ToValue()}-font", Space.IsUsed()),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<NavigationContext, NavigationTagHelper>(out NavigationContext navigationContext))
            {
                await output.GetChildContentAsync();

                if (Navs.Any())
                {
                    output.TagName = "ul";

                    output.TagMode = TagMode.StartTagAndEndTag;

                    output.Attributes.Add("id", Id);

                    output.Attributes.Add("class", Classes);

                    if (!String.IsNullOrWhiteSpace(Title))
                    {
                        output.Content.AppendHtml($"<li class='kt-nav__head'>{Title}</li>");

                        output.Content.AppendHtml($"<li class='kt-nav__separator'></li>");
                    }

                    var sections = Navs.Where(t => !string.IsNullOrWhiteSpace(t.Section)).OrderBy(t => t.Order).Select(t => t.Section).Distinct().ToList();

                    if (sections.Any())
                    {
                        for (var i = 0; i < sections.Count; i++)
                        {
                            var section = sections[i];

                            var sectionItem = new TagBuilder("div");

                            sectionItem.AddCssClass("kt-nav__section");

                            if (i == 0)
                            {
                                sectionItem.AddCssClass("kt-nav__section--first");
                            }

                            sectionItem.InnerHtml.AppendHtml($"<span class='kt-nav__section-text'>{section}</span>");

                            output.Content.AppendHtml(sectionItem);

                            var items = Navs.Where(t => t.Section == section);

                            BuildNavigation(output, items);
                        }
                    }
                    else
                    {
                        BuildNavigation(output, Navs);
                    }

                    if (navigationContext.Footer.IsNotNull())
                    {
                        output.Content.AppendHtml($"<li class='kt-nav__separator'></li>");

                        output.Content.AppendHtml(navigationContext.Footer);
                    }

                    return;
                }
            }

            output.SuppressOutput();
        }

        internal void BuildNavigation(TagHelperOutput output, IEnumerable<NavigationItem> navigationItems)
        {
            foreach (var item in navigationItems.OrderBy(t => t.Order))
            {
                var navigationItem = BuildNavigationItem(item);

                output.Content.AppendHtml(navigationItem);
            }
        }

        internal TagBuilder BuildNavigationItem(NavigationItem navigationItem)
        {
            var item = new TagBuilder("li");

            item.AddCssClass("kt-nav__item");

            if (navigationItem.IsActived)
            {
                item.AddCssClass("kt-nav__item--active");
            }

            var link = new TagBuilder("a");

            link.AddCssClass("kt-nav__link");

            link.InnerHtml.AppendHtml(BuildLinkBullet(navigationItem));

            if (!string.IsNullOrWhiteSpace(navigationItem.Text))
            {
                link.InnerHtml.AppendHtml($"<span class='kt-nav__link-text'>{navigationItem.Text}</span>");
            }

            if (!String.IsNullOrWhiteSpace(navigationItem.Badge))
            {
                link.InnerHtml.AppendHtml($@"<span class='kt-nav__link-badge'>
										 	    <span class='kt-badge kt-badge--{navigationItem.BadgeState.ToValue()} kt-badge--inline kt-badge--pill kt-badge--rounded'>{navigationItem.Badge}</span>
										     </span>");
            }

            var subNavId = $"nav-sub-{Guid.NewGuid()}";

            if (navigationItem.Navs.Any() && IsToggled)
            {
                link.Attributes.Add("data-toggle", "collapse");

                link.Attributes.Add("href", $"#{subNavId}");

                link.InnerHtml.AppendHtml("<span class='kt-nav__link-arrow'></span>");
            }
            else
            {
                link.Attributes.Add("href", "javascript:;");
            }

            item.InnerHtml.AppendHtml(link);

            if (navigationItem.Navs.Any())
            {
                var subNavigation = new TagBuilder("ul");

                subNavigation.AddCssClass("kt-nav__sub");

                if (IsToggled)
                {
                    subNavigation.Attributes.Add("id", subNavId);

                    subNavigation.Attributes.Add("data-parent", $"#{Id}");

                    subNavigation.AddCssClass("collapse show");
                }

                foreach (var subItem in navigationItem.Navs)
                {
                    var subNavigationItem = BuildNavigationItem(subItem);

                    subNavigation.InnerHtml.AppendHtml(subNavigationItem);
                }

                item.InnerHtml.AppendHtml(subNavigation);
            }

            return item;
        }

        internal string BuildLinkBullet(NavigationItem navigationItem)
        {
            var isUseIcon = navigationItem.Icon.IsUsed();

            var icon = isUseIcon ? navigationItem.Icon.ToIconContent("kt-nav__link-icon") : "<i class='kt-nav__link-icon'></i>";

            switch (Bullet)
            {
                case NavigationBullet.Dot:
                    return isUseIcon ? icon : "<span class='kt-nav__link-bullet kt-nav__link-bullet--line'><span></span></span>";
                case NavigationBullet.Line:
                    return isUseIcon ? icon : "<span class='kt-nav__link-bullet kt-nav__link-bullet--dot'><span></span></span>";
                default:
                    return icon;
            }
        }
    }
}
