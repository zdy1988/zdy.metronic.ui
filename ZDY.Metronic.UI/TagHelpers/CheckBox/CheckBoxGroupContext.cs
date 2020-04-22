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
    internal class CheckBoxGroupContext : ITagHelperContext
    {
        internal bool IsMultiSelect { get; set; } = true;

        internal string GroupId { get; set; }

        internal List<IHtmlContent> Boxes { get; set; } = new List<IHtmlContent>();
    }
}
