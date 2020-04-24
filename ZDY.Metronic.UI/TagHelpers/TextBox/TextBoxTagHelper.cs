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
    [HtmlTargetElement("text-box")]
    public class TextBoxTagHelper : FormGroupTagHelper
    {
        public virtual string Field { get; set; }

        public virtual TextBoxKinds Kind { get; set; } = TextBoxKinds.Text;

        public virtual Size Size { get; set; } = Size.None;

        public virtual TextBoxAddonAlign IconAlign { get; set; } = TextBoxAddonAlign.Left;

        public virtual string Placeholder { get; set; }

        public virtual bool IsDisabled { get; set; } = false;

        public virtual int Rows { get; set; } = 3;

        public virtual string PrependText { get; set; }

        public virtual string AppendText { get; set; }

        protected virtual string Input
        {
            get
            {
                return IsMultiple ? "textarea" : "input";
            }
        }

        protected virtual bool IsMultiple
        {
            get
            {
                return Kind == TextBoxKinds.Textarea;
            }
        }

        protected virtual string PlaceholderValue
        {
            get
            {
                return Placeholder ?? String.Format(Settings.GetInstance().TextBoxPlaceholderFormat, Name);
            }
        }

        protected override string HelpTextValue
        {
            get
            {
                return HelpText ?? String.Format(Settings.GetInstance().TextBoxHelpTextFormat, Name);
            }
        }

        protected virtual string InputClasses
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("form-control", true),
                    new CssClass($"form-control-{Size.ToValue()}", Size.IsUsed())
                );
            }
        }

        protected virtual string InputGroupClasses
        {
            get
            {
                if (Icon.IsUsed())
                {
                    return CssClassBuilder.Build(
                        new CssClass("kt-input-icon", true),
                        new CssClass($"kt-input-icon--{IconAlign.ToValue()}", true)
                    );
                }
                else
                {
                    return CssClassBuilder.Build(
                        new CssClass("input-group", true),
                        new CssClass($"input-group-{Size.ToValue()}", Size.IsUsed())
                    );
                }
            }
        }

        protected virtual string IconClasses
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-input-icon__icon", true),
                    new CssClass($"kt-input-icon__icon--{IconAlign.ToValue()}", true)
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<TextBoxContext, TextBoxTagHelper>(out TextBoxContext textBoxContext))
            {
                await output.GetChildContentAsync();

                var input = new TagBuilder(Input);

                input.Attributes.Add("id", Id);
                input.Attributes.Add("name", Field);
                input.Attributes.Add("placeholder", PlaceholderValue);
                input.Attributes.Add("class", InputClasses);

                if (IsDisabled)
                {
                    input.Attributes.Add("disabled", "disabled");
                }

                if (IsMultiple)
                {
                    input.Attributes.Add("rows", Rows.ToString());
                }
                else
                {
                    input.Attributes.Add("type", Kind.ToValue());
                }

                var inputGroup = BuildInputContainer(input, textBoxContext);

                if (context.TryGetContext<ButtonToolbarContext, ButtonToolbarTagHelper>(out ButtonToolbarContext buttonToolbarContext))
                {
                    buttonToolbarContext.Groups.Add(inputGroup);
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(Name))
                    {
                        output.TransformOutput(inputGroup);
                    }
                    else
                    {
                        base.OutputFormGroup(output, inputGroup);
                    }
                }

                return;
            }

            output.SuppressOutput();
        }

        internal TagBuilder BuildInputContainer(IHtmlContent input, TextBoxContext context)
        {
            var inputContainer = new TagBuilder("div");

            inputContainer.AddCssClass(InputGroupClasses);

            inputContainer.InnerHtml.AppendHtml(BuildPrependAddon(context));
            inputContainer.InnerHtml.AppendHtml(input);
            inputContainer.InnerHtml.AppendHtml(BuildAppendAddon(context));

            if (Icon.IsUsed())
            {
                inputContainer.InnerHtml.AppendHtml($@"<span class='{IconClasses}'><span>{Icon.ToIconContent()}</span></span>");
            }

            return inputContainer;
        }

        internal string BuildPrependAddon(TextBoxContext context)
        {
            if (Icon.IsUsed()) return "";

            var isPrepend = false;

            var prepend = new TagBuilder("div");

            prepend.Attributes.Add("class", "input-group-prepend");

            if (!String.IsNullOrWhiteSpace(PrependText))
            {
                prepend.InnerHtml.AppendHtml($@"<span class='input-group-text'>{PrependText}</span>");

                isPrepend = true;
            }

            if (context.PrependAddons.Any())
            {
                foreach (var addon in context.PrependAddons)
                {
                    prepend.InnerHtml.AppendHtml(addon);
                }

                isPrepend = true;
            }

            return isPrepend ? prepend.ToHtml() : "";
        }

        internal string BuildAppendAddon(TextBoxContext context)
        {
            if (Icon.IsUsed()) return "";

            var isAppend = false;

            var append = new TagBuilder("div");

            append.Attributes.Add("class", "input-group-append");

            if (!String.IsNullOrWhiteSpace(AppendText))
            {
                append.InnerHtml.AppendHtml($@"<span class='input-group-text'>{AppendText}</span>");

                isAppend = true;
            }

            if (context.AppendAddons.Any())
            {
                foreach (var addon in context.AppendAddons)
                {
                    append.InnerHtml.AppendHtml(addon);
                }

                isAppend = true;
            }

            return isAppend ? append.ToHtml() : "";
        }
    }
}
