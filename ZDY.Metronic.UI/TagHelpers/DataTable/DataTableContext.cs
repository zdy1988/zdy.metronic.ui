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
    internal class DataTableContext : IHelperContext
    {
        internal List<DataTableCheckboxColumnTagHelper> CheckboxColumns { get; set; } = new List<DataTableCheckboxColumnTagHelper>();

        internal List<Tuple<DataTableTemplateColumnTagHelper, IHtmlContent>> TemplateColumns { get; set; } = new List<Tuple<DataTableTemplateColumnTagHelper, IHtmlContent>>();
    }
}
