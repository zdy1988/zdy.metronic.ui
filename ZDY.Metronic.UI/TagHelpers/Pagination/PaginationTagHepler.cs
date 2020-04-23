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
    [HtmlTargetElement("pagination", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PaginationTagHepler : HelperBase
    {
        public virtual State State { get; set; } = State.None;

        public virtual Size Size { get; set; } = Size.None;

        public virtual bool IsCircled { get; set; } = false;

        public virtual int PageIndex { get; set; }

        public virtual int PageSize { get; set; }

        public virtual int Total { get; set; }

        public virtual int Limit { get; set; } = 6;

        public virtual string QueryString { get; set; } = "page";

        public virtual string Url { get; set; }

        public virtual string UrlFormat { get; set; }

        public virtual string DescFormat { get; set; } = "Showing {0} - {1} Of {2} Records";

        protected virtual string StateValue
        {
            get
            {
                return State.IsUsed() && State != State.Light ? State.ToValue() : "brand";
            }
        }

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-pagination", true),
                    new CssClass("kt-pagination--circle", IsCircled),
                    new CssClass($"kt-pagination--{Size.ToValue()}", Size.IsUsed()),
                    new CssClass($"kt-pagination--{StateValue}", true),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Total > 0)
            {
                output.TagName = "div";

                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Add("id", Id);

                output.Attributes.Add("class", Classes);

                output.Content.AppendHtml(BuildTagLinks());

                output.Content.AppendHtml(BuildToolbar());

                return;
            }

            output.SuppressOutput();
        }

        internal string BuildUrl(int index)
        {
            if (string.IsNullOrWhiteSpace(UrlFormat))
            {
                Url = string.IsNullOrWhiteSpace(Url) ? "#" : Url;

                return $"{Url}?{QueryString}={index}";
            }
            else
            {
                return String.Format(UrlFormat, index);
            }
        }

        internal TagBuilder BuildTagLinks()
        {
            var links = new TagBuilder("ul");

            links.AddCssClass("kt-pagination__links");

            PageSize = PageSize <= 0 ? 10 : PageSize;

            PageIndex = PageIndex <= 0 ? 1 : PageIndex;

            // 计算分页
            var linkCount = Total / PageSize + (Total % PageSize > 0 ? 1 : 0);

            int linkStart = linkCount - PageIndex < Limit ? linkCount - Limit + 1 : (PageIndex >= Limit ? PageIndex - Limit / 2 : 1);

            int linkEnd = linkStart + Limit - 1 > linkCount ? linkCount : linkStart + Limit - 1;


            if (PageIndex != 1)
            {
                links.InnerHtml.AppendHtml($@"<li class='kt-pagination__link--first'>
											      <a href='{BuildUrl(1)}'><i class='fa fa-angle-double-left kt-font-{StateValue}'></i></a>
											  </li>");
            }

            if (PageIndex - 1 >= 1)
            {
                links.InnerHtml.AppendHtml($@"<li class='kt-pagination__link--prev'>
											      <a href='{BuildUrl(PageIndex - 1)}'><i class='fa fa-angle-left kt-font-{StateValue}'></i></a>
											  </li>");
            }

            if (PageIndex >= Limit)
            {
                var index = linkStart - Limit / 2 < 1 ? 1 : linkStart - Limit / 2;

                links.InnerHtml.AppendHtml($@"<li class='kt-pagination__link--prev-ellipsis'>
                                                  <a href='{BuildUrl(index)}'>...</a>
                                              </li>");
            }

            for (var i = linkStart; i <= linkEnd; i++)
            {
                var activeClass = i == PageIndex ? "kt-pagination__link--active" : "";

                links.InnerHtml.AppendHtml($@"<li class='{activeClass}'>
											  	  <a href='{BuildUrl(i)}'>{i}</a>
											  </li>");
            }

            if (linkEnd < linkCount)
            {
                var index = linkEnd + Limit / 2 > linkCount ? linkCount : linkEnd + Limit / 2;

                links.InnerHtml.AppendHtml($@"<li class='kt-pagination__link--next-ellipsis'>
                                                  <a href='{BuildUrl(index)}'>...</a>
                                              </li>");
            }

            if (PageIndex + 1 <= linkCount)
            {
                links.InnerHtml.AppendHtml($@"<li class='kt-pagination__link--next'>
											  	  <a href='{BuildUrl(PageIndex + 1)}'><i class='fa fa-angle-right kt-font-{StateValue}'></i></a>
											  </li>");
            }

            if (PageIndex != linkCount)
            {
                links.InnerHtml.AppendHtml($@"<li class='kt-pagination__link--last'>
											      <a href='{BuildUrl(linkCount)}'><i class='fa fa-angle-double-right kt-font-{StateValue}'></i></a>
											  </li>");
            }

            return links;
        }

        internal TagBuilder BuildToolbar()
        {
            var toolbar = new TagBuilder("div");

            toolbar.AddCssClass("kt-pagination__toolbar");

            toolbar.InnerHtml.AppendHtml($@"<select class='form-control kt-font-{StateValue}' style='width: 60px'>
												<option value='10'>10</option>
												<option value='20'>20</option>
												<option value='30'>30</option>
												<option value='50'>50</option>
												<option value='100'>100</option>
											</select>
                                            <span class='pagination__desc'>
                                                {String.Format(DescFormat, (PageIndex - 1) * PageSize + 1, PageIndex * PageSize, Total)}
                                            </span>");
            return toolbar;
        }
    }
}
