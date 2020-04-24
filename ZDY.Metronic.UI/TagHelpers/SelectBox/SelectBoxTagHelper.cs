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
    [HtmlTargetElement("select-box", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SelectBoxTagHelper : TextBoxTagHelper
    {
        public virtual List<SelectOption> Options { get; set; }

        public virtual bool IsPlaceHolderDestroyed { get; set; } = false;

        public override int Rows { get; set; }

        protected override string PlaceholderValue
        {
            get
            {
                return Placeholder ?? String.Format(Settings.GetInstance().SelectBoxPlaceholderFormat, Name);
            }
        }

        protected override string HelpTextValue
        {
            get
            {
                return Placeholder ?? String.Format(Settings.GetInstance().SelectBoxHelpTextFormat, Name);
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<SelectBoxContext, SelectBoxTagHelper>(out SelectBoxContext selectBoxContext) &&
                context.TryAddContext<TextBoxContext, TextBoxTagHelper>(out TextBoxContext textBoxContext))
            {
                await output.GetChildContentAsync();

                if (Options.IsNotNull())
                {
                    Options.AddRange(selectBoxContext.Options);
                }
                else
                {
                    Options = selectBoxContext.Options;
                }

                if (Options.Any())
                {
                    Options = Options.OrderBy(t => t.Order).ToList();

                    var select = new TagBuilder("select");

                    select.Attributes.Add("id", Id);
                    select.Attributes.Add("name", Field);
                    select.Attributes.Add("class", InputClasses);

                    if (IsDisabled)
                    {
                        select.Attributes.Add("disabled", "disabled");
                    }

                    if (Rows > 0)
                    {
                        select.Attributes.Add("multiple", "multiple");
                        select.Attributes.Add("size", Rows.ToString());
                    }

                    if (!IsPlaceHolderDestroyed&& Rows <= 0)
                    {
                        select.InnerHtml.AppendHtml($"<option value=''>{PlaceholderValue}</option>");
                    }

                    var sections = Options.Where(t => !String.IsNullOrWhiteSpace(t.Section)).Select(t => t.Section).Distinct();

                    if (sections.Any())
                    {
                        foreach (var section in sections)
                        {
                            var optgroup = new TagBuilder("optgroup");

                            optgroup.Attributes.Add("label", section);

                            foreach (var option in Options.Where(t => t.Section == section))
                            {
                                optgroup.InnerHtml.AppendHtml($"<option value='{option.Value}'>{option.Name}</option>");
                            }

                            select.InnerHtml.AppendHtml(optgroup);
                        }
                    }
                    else
                    {
                        foreach (var option in Options)
                        {
                            select.InnerHtml.AppendHtml($"<option value='{option.Value}'>{option.Name}</option>");
                        }
                    }

                    var inputGroup = BuildInputContainer(select, textBoxContext);

                    if (String.IsNullOrWhiteSpace(Name))
                    {
                        output.TransformOutput(inputGroup);
                    }
                    else
                    {
                        base.OutputFormGroup(output, inputGroup);
                    }

                    return;
                }
            }

            output.SuppressOutput();
        }
    }
}
