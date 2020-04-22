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
    [HtmlTargetElement("icon-flat2", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class Flat2IconTagHelper : BaseIconTagHelper<Flat2Icon>
    {
        protected override string GetIcon() => Flat2.Get(Name);
    }
}
