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
    [HtmlTargetElement("form-group")]
    public class FormGroupTagHelper : HelperBase<LineawesomeIcon>
    {
        public virtual string Name { get; set; }

        public virtual string HelpText { get; set; }

        public virtual bool IsHelpTextDestroyed { get; set; } = Settings.GetInstance().IsFormBoxHelpTextDestroyed;

        public virtual FormGroupMode GroupMode { get; set; } = FormGroupMode.Vertical;

        public virtual bool IsRequired { get; set; } = false;

        protected virtual string NameValue
        {
            get
            {
                return IsRequired ? $"{Name} *" : Name;
            }
        }

        protected virtual string HelpTextValue
        {
            get
            {
                return HelpText ?? $"Please Enter {Name}...";  
            }
        }

        protected virtual bool IsVertical
        {
            get
            {
                return GroupMode == FormGroupMode.Vertical;
            }
        }

        protected virtual bool IsCompact {
            get
            { 
                return GroupMode == FormGroupMode.Compact;
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            if (!childContent.IsEmptyOrWhiteSpace)
            {
                OutputFormGroup(output, childContent);

                return;
            }

            output.SuppressOutput();
        }

        internal void OutputFormGroup(TagHelperOutput output, IHtmlContent content)
        {
            var container = IsVertical ? BuildVerticalContainer(content) : BuildHorizontalContainer(content);

            output.TransformOutput(container);
        }

        internal TagBuilder BuildVerticalContainer(IHtmlContent inputGroup)
        {
            var container = new TagBuilder("div");

            container.Attributes.Add("class", "form-group");

            container.InnerHtml.AppendHtml($"<label class='form-control-label'>{NameValue}</label>");

            container.InnerHtml.AppendHtml(inputGroup.ToHtml());

            if (!IsHelpTextDestroyed)
            {
                container.InnerHtml.AppendHtml($"<span class='form-text text-muted'>{HelpTextValue}</span>");
            }

            return container;
        }

        internal TagBuilder BuildHorizontalContainer(IHtmlContent inputGroup)
        {
            var container = new TagBuilder("div");

            container.Attributes.Add("class", "form-group row");

            container.InnerHtml.AppendHtml(
                $@"<label class='col-sm-3 col-form-label {(IsCompact ? "text-sm-right" : "")}'>
                       {NameValue}
                   </label>
				   <div class='{(IsCompact ? "col-sm-6" : "col-sm-9")}'>
                          {inputGroup.ToHtml()}
                          <span class='form-text text-muted'>{HelpTextValue}</span>
				   </div>");

            return container;
        }
    }
}
