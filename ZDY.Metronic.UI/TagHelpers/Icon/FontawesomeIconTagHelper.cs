﻿using System;
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
    [HtmlTargetElement("icon-fa", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FontawesomeIconTagHelper : BaseIconTagHelper<FontawesomeIcon>
    {
        protected override string GetIcon() => Fontawesome.Get(Name);
    }
}
