using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZDY.Metronic.UI.Untils;
using ZDY.Metronic.UI.Icons;

namespace ZDY.Metronic.UI.TagHelpers
{
    [HtmlTargetElement("icon-line", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class LineawesomeIconTagHelper : BaseIconTagHelper<LineawesomeIcon>
    {
        protected override string GetIcon() => Lineawesome.Get(Name);
    }
}
