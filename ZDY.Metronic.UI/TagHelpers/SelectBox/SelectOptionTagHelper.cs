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
    [HtmlTargetElement("select-option", ParentTag = "select-box", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SelectOptionTagHelper : BaseTagHelper
    {
        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public virtual string Section { get; set; }

        public virtual int Index { get; set; } = 0;

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<SelectBoxContext, SelectBoxTagHelper>(out SelectBoxContext selectBoxContext))
            {
                var childContent = await output.GetChildContentAsync();

                Name = String.IsNullOrWhiteSpace(Name) ? childContent.GetContent() : Name;

                Value = String.IsNullOrWhiteSpace(Value) ? Name : Value;

                selectBoxContext.Options.Add(new SelectOption
                {
                    Name = Name,
                    Value = Value,
                    Order = Index,
                    Section = Section
                });
            }
        }
    }
}
