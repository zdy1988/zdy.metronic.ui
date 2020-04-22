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
    [HtmlTargetElement("check-box-group")]
    public class CheckBoxGroupTagHelper : FormGroupTagHelper
    {
        public virtual bool IsInline { get; set; } = false;

        public virtual bool IsMultiSelect { get; set; } = true;

        protected virtual string CheckInput
        {
            get
            {
                return IsMultiSelect ? "checkbox" : "radio";
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<CheckBoxGroupContext, CheckBoxGroupTagHelper>(out CheckBoxGroupContext checkboxGroupContext))
            {
                checkboxGroupContext.IsMultiSelect = IsMultiSelect;

                checkboxGroupContext.GroupId = Id;

                await output.GetChildContentAsync();

                var group = new TagBuilder("div");

                group.Attributes.Add("id", Id);

                group.AddCssClass(IsInline ? $"kt-{CheckInput}-inline" : $"kt-{CheckInput}-list");

                if (checkboxGroupContext.Boxes.Any())
                {
                    foreach (var box in checkboxGroupContext.Boxes)
                    {
                        group.InnerHtml.AppendHtml(box);
                    }
                }

                if (String.IsNullOrWhiteSpace(Name))
                {
                    output.TransformOutput(group);
                }
                else
                {
                    base.OutputFormGroup(output, group);
                }

                return;
            }

            output.SuppressOutput();
        }
    }
}
