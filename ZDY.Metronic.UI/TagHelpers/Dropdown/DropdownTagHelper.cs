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
    [HtmlTargetElement("dropdown")]
    public class DropdownTagHelper : HelperBase<Flat2Icon>
    {
        public virtual string Text { get; set; } = "Dropdown";

        public virtual State State { get; set; } = State.None;

        public virtual Size Size { get; set; } = Size.None;

        public virtual Fit Fit { get; set; } = Fit.None;

        public virtual DropdownMode Mode { get; set; } = DropdownMode.None;

        public virtual DropdownDirection Direction { get; set; } = DropdownDirection.Down;

        public virtual Size ButtonSize { get; set; } = Size.None;

        public virtual ButtonShape ButtonShape { get; set; } = ButtonShape.None;

        public override Flat2Icon Icon { get; set; } = Flat2Icon.More1;

        public virtual State IconState { get; set; } = State.None;

        public virtual Size IconSize { get; set; } = Size.Md;

        public virtual bool IsCleaned { get; set; } = false;

        public virtual bool IsOutline { get; set; } = false;

        protected virtual string StateValue
        {
            get
            {
                if (IsCleaned)
                {
                    return "clean";
                }

                var state = State.IsUsed() ? State.ToValue() : "secondary";

                if (IsOutline)
                {
                    return $"outline-{state}";
                }

                return state;
            }
        }

        protected virtual string DropdownClasses
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("btn-group", Mode != DropdownMode.Icon),
                    new CssClass("dropdown dropdown-inline", Mode == DropdownMode.Icon),
                    new CssClass($"drop{Direction.ToValue()}", Direction.IsUsed()),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        protected virtual string DropdownMenuClasses
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("dropdown-menu", true),
                    new CssClass($"dropdown-menu-{Size.ToValue()}", Size.IsUsed()),
                    new CssClass($"dropdown-menu-{Fit.ToValue()}", Fit.IsUsed())
                );
            }
        }

        protected virtual string DropdownButtonClasses
        {
            get
            {
                return CssClassBuilder.Build(
                   new CssClass("btn", true),
                   new CssClass("dropdown-toggle", Mode != DropdownMode.Icon),
                   new CssClass("dropdown-toggle-split", Mode == DropdownMode.Split),
                   new CssClass($"btn-{StateValue}", true),
                   new CssClass($"btn-{ButtonSize.ToValue()}", ButtonSize.IsUsed()),
                   new CssClass($"btn-{ButtonShape.ToValue()}", ButtonShape.IsUsed()),
                   new CssClass("btn-icon", Mode == DropdownMode.Icon),
                   new CssClass($"btn-icon-{IconSize.ToValue()}", Mode == DropdownMode.Icon && IconSize.IsUsed())
                );
            }
        }

        protected virtual string SplitButtonClasses
        {
            get
            {
                return CssClassBuilder.Build(
                   new CssClass("btn", true),
                   new CssClass($"btn-{StateValue}", true),
                   new CssClass($"btn-{ButtonSize.ToValue()}", ButtonSize.IsUsed()),
                   new CssClass($"btn-{ButtonShape.ToValue()}", ButtonShape.IsUsed())
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent= await output.GetChildContentAsync();

            output.TagName = "div";

            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", Id);

            output.Attributes.Add("class", DropdownClasses);

            output.Attributes.Add("role", "group");

            if (Mode == DropdownMode.Split)
            {
                output.Content.AppendHtml($"<button type='button' class='{SplitButtonClasses}'>{Text}</button>");
            }

            output.Content.AppendHtml(BuildDropdownButton());

            output.Content.AppendHtml(BuildDropdownMenu(childContent));
        }

        internal virtual TagBuilder BuildDropdownButton()
        {
            var dropdownButton = new TagBuilder("button");

            dropdownButton.Attributes.Add("id", Id);
            dropdownButton.Attributes.Add("type", "button");
            dropdownButton.Attributes.Add("class", DropdownButtonClasses);
            dropdownButton.Attributes.Add("data-toggle", "dropdown");
            dropdownButton.Attributes.Add("aria-haspopup", "true");
            dropdownButton.Attributes.Add("aria-expanded", "false");

            if (Mode == DropdownMode.Icon)
            {
                var iconClasses = IconState.IsUsed() ? $"kt-font-{IconState.ToValue()}" : "";

                dropdownButton.InnerHtml.AppendHtml(Icon.ToIconContent(iconClasses));
            }
            else if (Mode == DropdownMode.Split)
            {
                dropdownButton.InnerHtml.AppendHtml("<span class='sr-only'>Toggle Dropdown</span>");
            }
            else
            {
                dropdownButton.InnerHtml.Append(Text);
            }

            return dropdownButton;
        }

        internal virtual TagBuilder BuildDropdownMenu(TagHelperContent childContent)
        {
            var dropdownMenu = new TagBuilder("div");

            dropdownMenu.AddCssClass(DropdownMenuClasses);

            dropdownMenu.Attributes.Add("aria-labelledby", Id);

            if (childContent.IsNotNull())
            {
                dropdownMenu.InnerHtml.AppendHtml(childContent);
            }

            return dropdownMenu;
        }
    }
}
