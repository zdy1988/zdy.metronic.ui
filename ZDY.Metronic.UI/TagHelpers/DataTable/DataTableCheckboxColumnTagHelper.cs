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
    [HtmlTargetElement("checkbox-column", ParentTag = "data-table", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DataTableCheckboxColumnTagHelper : BaseTagHelper
    {
        public virtual string FieldName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<DataTableContext, DataTableTagHelper>(out DataTableContext dataTableContext))
            {
                dataTableContext.CheckboxColumns.Add(this);
            }
        }
    }
}
