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
    [HtmlTargetElement("check-box", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CheckBoxTagHelper : FormGroupTagHelper
    {
        public virtual string Text { get; set; }

        public virtual string Value { get; set; }

        public virtual State State { get; set; } = State.None;

        public virtual CheckBoxMode Mode { get; set; } = CheckBoxMode.None;

        public virtual bool IsDisabled { get; set; } = false;

        public virtual bool IsChecked { get; set; } = false;

        public virtual bool IsMultiSelect { get; set; } = true;

        protected virtual string CheckInput
        {
            get
            {
                return IsMultiSelect ? "checkbox" : "radio";
            }
        }

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass($"kt-{CheckInput}", true),
                    new CssClass($"kt-{CheckInput}--{State.ToValue()}", State.IsUsed()),
                    new CssClass($"kt-{CheckInput}--{Mode.ToValue()}", Mode.IsUsed()),
                    new CssClass($"kt-{CheckInput}--disabled", IsDisabled),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();

            if (context.TryGetContext<CheckBoxGroupContext, CheckBoxGroupTagHelper>(out CheckBoxGroupContext checkboxGroupContext))
            {
                IsMultiSelect = checkboxGroupContext.IsMultiSelect;

                TagBuilder box = BuildBox(checkboxGroupContext.GroupId);

                checkboxGroupContext.Boxes.Add(box);

                output.SuppressOutput();
            }
            else
            {
                TagBuilder box = BuildBox();

                if (String.IsNullOrWhiteSpace(Name))
                {
                    output.TransformOutput(box);
                }
                else
                {
                    var div = new TagBuilder("div");

                    box.Attributes.Add("style", "margin-bottom: 5px;");

                    div.InnerHtml.AppendHtml(box);

                    base.OutputFormGroup(output, div);
                }
            }
        }

        internal TagBuilder BuildBox(string name = default)
        {
            TagBuilder box = new TagBuilder("label"); ;

            box.AddCssClass(Classes);

            var input = new TagBuilder("input");

            input.Attributes.Add("id", Id);

            input.Attributes.Add("type", CheckInput);

            if (!IsMultiSelect && !String.IsNullOrWhiteSpace(name))
            {
                input.Attributes.Add("name", name);
            }

            if (IsChecked)
            {
                input.Attributes.Add("checked", "checked");
            }

            if (IsDisabled)
            {
                input.Attributes.Add("disabled", "disabled");
            }

            if (!String.IsNullOrEmpty(Value))
            {
                input.Attributes.Add("value", Value);
            }

            box.InnerHtml.AppendHtml(input);

            if (!String.IsNullOrEmpty(Text))
            {
                box.InnerHtml.AppendHtml(Text);
            }
            else
            {
                box.InnerHtml.AppendHtml("&nbsp;");
            }

            box.InnerHtml.AppendHtml("<span></span>");

            return box;
        }
    }
}
