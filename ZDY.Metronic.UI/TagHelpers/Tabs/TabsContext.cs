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
    internal class TabsContext : ITagHelperContext
    {
        internal List<Tuple<TabPaneTagHelper, IHtmlContent>> TabPanes { get; set; } = new List<Tuple<TabPaneTagHelper, IHtmlContent>>();
    }
}
