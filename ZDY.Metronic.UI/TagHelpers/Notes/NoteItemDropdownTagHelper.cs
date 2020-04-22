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
    [HtmlTargetElement("note-item-dropdown", ParentTag = "note-item")]
    public class NoteItemDropdownTagHelper : ButtonDropdownTagHelper
    {
        public override DropdownMode Mode { get; set; } = DropdownMode.Icon;

        public override Size ButtonSize { get; set; } = Size.Sm;

        public override Size IconSize { get; set; } = Size.Md;

        public override State State { get; set; } = State.Light;

        public override State IconState { get; set; } = State.Brand;

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryAddContext<ButtonDropdownContext, ButtonDropdownTagHelper>(out ButtonDropdownContext buttonDropdownContext))
            {
                await output.GetChildContentAsync();

                if (buttonDropdownContext.ButtonOrDividers.Any())
                {
                    var dropdown = new TagBuilder("div");

                    dropdown.Attributes.Add("class", "kt-notes__dropdown");

                    dropdown.InnerHtml.AppendHtml(BuildDropdownButton());
                    dropdown.InnerHtml.AppendHtml(BuildDropdownMenu(buttonDropdownContext));

                    if (context.TryGetContext<NoteItemContext, NoteItemTagHelper>(out NoteItemContext noteItemContext))
                    {
                        noteItemContext.Dropdown = dropdown;
                    }
                }
            }
        }

        internal override TagBuilder BuildDropdownMenu(ButtonDropdownContext buttonDropdownContext)
        {
            var dropdownMenu = base.BuildDropdownMenu(buttonDropdownContext);

            dropdownMenu.AddCssClass("dropdown-menu-right");

            return dropdownMenu;
        }
    }
}
