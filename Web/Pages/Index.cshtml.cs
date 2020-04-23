using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        public List<MultiLevelPage> Pages { get; set; } = new List<MultiLevelPage>();

        Random random = new Random();

        public void OnGet()
        {
            var pages = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => typeof(PageModel).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic))
                .Select(t => new MultiLevelPage { Name = t.Name.Replace("Model", "") })
                .ToList();

            Pages.Add(new MultiLevelPage { Name = "Dashboard", IsActived = true });

            Pages.Add(new MultiLevelPage { Name = "Custom", IsActived = true, IsSection = true });

            Pages.Add(new MultiLevelPage { Name = "Pages", ChildLevelPages = pages });

            Pages.Add(new MultiLevelPage { Name = "Example 1", IsActived = true, IsSection = true });

            for (var i = 0; i < 5; i++)
            {
                Pages.Add(new MultiLevelPage
                {
                    Name = $"Example {i}",
                    ChildLevelPages = new List<MultiLevelPage> {
                        new MultiLevelPage{ Name = $"Example {i} - 1"},
                        new MultiLevelPage{ Name = $"Example {i} - 2"},
                        new MultiLevelPage{ Name = $"Example {i} - 3", ChildLevelPages = new List<MultiLevelPage> {
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 1"},
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 2"},
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 3"},
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 4"},
                        }},
                        new MultiLevelPage{ Name = $"Example {i} - 4"}
                    }
                });
            }

            Pages.Add(new MultiLevelPage { Name = "Example 2", IsActived = true, IsSection = true });

            for (var i = 0; i < 5; i++)
            {
                Pages.Add(new MultiLevelPage
                {
                    Name = $"Example {i}",
                    ChildLevelPages = new List<MultiLevelPage> {
                        new MultiLevelPage{ Name = $"Example {i} - 1"},
                        new MultiLevelPage{ Name = $"Example {i} - 2"},
                        new MultiLevelPage{ Name = $"Example {i} - 3", ChildLevelPages = new List<MultiLevelPage> {
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 1"},
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 2"},
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 3"},
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 4"},
                        }},
                        new MultiLevelPage{ Name = $"Example {i} - 4"}
                    }
                });
            }

            Pages.Add(new MultiLevelPage { Name = "Example 3", IsActived = true, IsSection = true });

            for (var i = 0; i < 5; i++)
            {
                Pages.Add(new MultiLevelPage
                {
                    Name = $"Example {i}",
                    ChildLevelPages = new List<MultiLevelPage> {
                        new MultiLevelPage{ Name = $"Example {i} - 1"},
                        new MultiLevelPage{ Name = $"Example {i} - 2"},
                        new MultiLevelPage{ Name = $"Example {i} - 3", ChildLevelPages = new List<MultiLevelPage> {
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 1"},
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 2"},
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 3"},
                            new MultiLevelPage{ Name = $"Example {i} - 3 - 4"},
                        }},
                        new MultiLevelPage{ Name = $"Example {i} - 4"}
                    }
                });
            }
        }

        public TagBuilder RenderMenu(List<MultiLevelPage>  pages, bool isSubMenu = false)
        {
            var menu = new TagBuilder("div");

            if (!isSubMenu)
            {
                menu.AddCssClass("kt-aside-menu");
                menu.Attributes.Add("id", "kt_aside_menu");
                menu.Attributes.Add("data-ktmenu-vertical", "1");
                menu.Attributes.Add("data-ktmenu-scroll", "1");
                menu.Attributes.Add("data-ktmenu-dropdown-timeout", "500");
            }
            else
            {
                menu.AddCssClass("kt-menu__submenu");

                menu.InnerHtml.AppendHtml("<span class='kt-menu__arrow'></span>");
            }

            var nav = new TagBuilder("ul");

            if (!isSubMenu)
            {
                nav.AddCssClass("kt-menu__nav");
            }
            else
            {
                nav.AddCssClass("kt-menu__subnav");
            }

            foreach (var page in pages)
            {
                var item = new TagBuilder("li");

                if (page.IsSection)
                {
                    item.AddCssClass("kt-menu__section");

                    item.InnerHtml.AppendHtml($"<h4 class='kt-menu__section-text'>{page.Name}</h4>");
                }
                else
                {
                    var isBuildSubMenu = page.ChildLevelPages != null && page.ChildLevelPages.Any();

                    item.AddCssClass("kt-menu__item");

                    if (page.IsActived)
                    {
                        item.AddCssClass("kt-menu__item--active");
                    }

                    if (isBuildSubMenu)
                    {
                        item.AddCssClass("kt-menu__item--submenu");
                    }

                    var link = new TagBuilder("a");

                    link.AddCssClass("kt-menu__link");

                    link.Attributes.Add("id", page.Id.ToString());

                    if (isSubMenu)
                    {
                        var bulletClass = isBuildSubMenu ? "kt-menu__link-bullet--line" : "kt-menu__link-bullet--dot";

                        link.InnerHtml.AppendHtml($"<i class='kt-menu__link-bullet {bulletClass}'><span></span></i>");
                    }
                    else
                    {
                        var icon = (ZDY.Metronic.UI.SvgIcon)random.Next(1, 600);

                        var iconContent = ZDY.Metronic.UI.Tools.GetIconContent(icon);

                        link.InnerHtml.AppendHtml($"<span class='kt-menu__link-icon'>{iconContent}</span>");
                    }

                    link.InnerHtml.AppendHtml($"<span class='kt-menu__link-text'>{page.Name}</span>");

                    if (isBuildSubMenu)
                    {
                        link.Attributes.Add("href", "javascript:;");

                        link.AddCssClass("kt-menu__toggle");

                        link.InnerHtml.AppendHtml("<i class='kt-menu__ver-arrow la la-angle-right'></i>");

                        item.InnerHtml.AppendHtml(link);

                        item.InnerHtml.AppendHtml(RenderMenu(page.ChildLevelPages, true));
                    }
                    else
                    {
                        var data = new
                        {
                            PageId = page.Id,
                            PageSrc = $"/{page.Name}",
                            MenuName = page.Name
                        };

                        link.Attributes.Add("data-page", JsonConvert.SerializeObject(data));

                        link.Attributes.Add("href", $"/{page.Name}");

                        item.InnerHtml.AppendHtml(link);
                    }
                }

                nav.InnerHtml.AppendHtml(item);
            }

            menu.InnerHtml.AppendHtml(nav);

            return menu;
        }
    }

    public class MultiLevelPage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public String Name { get; set; }

        public bool IsActived { get; set; }

        public bool IsSection { get; set; }

        public List<MultiLevelPage> ChildLevelPages { get; set; } 
    }
}
