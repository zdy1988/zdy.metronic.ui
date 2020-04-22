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
    [HtmlTargetElement("button-dropdown")]
    public class ButtonDropdownTagHelper : DropdownTagHelper
    {
        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<ButtonDropdownContext, ButtonDropdownTagHelper>(out ButtonDropdownContext buttonDropdownContext))
            {
                await output.GetChildContentAsync();

                if (buttonDropdownContext.ButtonOrDividers.Any())
                {
                    var dropdown = new TagBuilder("div");

                    dropdown.Attributes.Add("class", DropdownClasses);
                    dropdown.Attributes.Add("role", "group");

                    if (Mode == DropdownMode.Split)
                    {
                        dropdown.InnerHtml.AppendHtml(BuildSplitLeftButton(buttonDropdownContext));
                    }

                    dropdown.InnerHtml.AppendHtml(BuildDropdownButton());
                    dropdown.InnerHtml.AppendHtml(BuildDropdownMenu(buttonDropdownContext));

                    if (context.TryGetContext<ButtonGroupContext, ButtonGroupTagHelper>(out ButtonGroupContext buttonGroupContext))
                    {
                        buttonGroupContext.ButtonOrDropdowns.Add(dropdown);
                    }
                    else
                    {
                        output.TransformOutput(dropdown);
                    }

                    return;
                }
            }

            output.SuppressOutput();
        }

        internal virtual TagBuilder BuildDropdownMenu(ButtonDropdownContext buttonDropdownContext)
        {
            var dropdownMenu = base.BuildDropdownMenu(null);

            foreach (var child in buttonDropdownContext.ButtonOrDividers)
            {
                var dropdownItem = (TagBuilder)child;

                var dropdownItemClasses = dropdownItem.Attributes["class"];

                if (!dropdownItemClasses.Contains("dropdown-divider"))
                {
                    if (dropdownItemClasses.Contains("btn-default"))
                    {
                        dropdownItemClasses = dropdownItemClasses.Replace("btn-default", "btn-light");
                    }

                    dropdownItem.Attributes["class"] = $"dropdown-item btn-square {dropdownItemClasses}";
                }

                dropdownMenu.InnerHtml.AppendHtml(dropdownItem.ToHtml());
            }

            return dropdownMenu;
        }

        internal virtual TagBuilder BuildSplitLeftButton(ButtonDropdownContext buttonDropdownContext)
        {
            var leftButton = buttonDropdownContext.ButtonOrDividers.Select(t => (TagBuilder)t).Where(t => t.Attributes["class"].Contains("btn")).FirstOrDefault();

            if (leftButton.IsNotNull())
            {
                buttonDropdownContext.ButtonOrDividers.Remove(leftButton);

                leftButton.Attributes["class"] = SplitButtonClasses;
            }

            return leftButton;
        }
    }
}
