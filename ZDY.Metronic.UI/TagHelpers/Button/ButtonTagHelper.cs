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
    [HtmlTargetElement("button", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ButtonTagHelper : BaseTagHelper<FontawesomeIcon>
    {
        public virtual ButtonKind Kind { get; set; } = ButtonKind.Button;

        public virtual string Text { get; set; }

        public virtual string Url { get; set; }

        public virtual Size Size { get; set; } = Size.None;

        public virtual State State { get; set; } = State.None;

        public virtual ButtonShape Shape { get; set; } = ButtonShape.None;

        public virtual ButtonTall Tall { get; set; } = ButtonTall.None;

        public virtual ButtonWide Wide { get; set; } = ButtonWide.None;

        public virtual ButtonState ButtonState { get; set; } = ButtonState.None;

        public virtual ButtonHoverMode HoverMode { get; set; } = ButtonHoverMode.None;

        public virtual Size FontSize { get; set; } = Size.None;

        public virtual FontWeight FontWeight { get; set; } = FontWeight.None;

        public virtual bool IsCleaned { get; set; } = false;

        public virtual bool IsOutline { get; set; } = false;

        public virtual bool IsOnlyIcon { get; set; } = false;

        public virtual Size OnlyIconSize { get; set; } = Size.None;

        public virtual bool IsDisabled { get; set; } = false;

        public virtual bool IsTextUpper { get; set; } = false;

        public virtual bool IsTextLower { get; set; } = false;

        protected virtual string KindValue
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(Url))
                {
                    return "a";
                }

                switch (Kind)
                {
                    case ButtonKind.Link:

                        if (String.IsNullOrWhiteSpace(Url))
                        {
                            Url = "javascript:;";
                        }

                        return "a";
                    case ButtonKind.Lable:
                        return "span";
                    default:
                        return "button";
                }
            }
        }

        protected virtual string ShapeValue
        {
            get
            {
                if (IsOnlyIcon && Shape == ButtonShape.Pill)
                {
                    return "circle";
                }

                return Shape.ToValue();
            }
        }

        protected virtual string StateValue
        {
            get
            {
                if (IsCleaned)
                {
                    return "clean";
                }

                if (HoverMode == ButtonHoverMode.Outline)
                {
                    return "none";
                }

                var state = State.IsUsed() ? State.ToValue() : "default";

                if (Kind == ButtonKind.Lable)
                {
                    return $"label-{state}";
                }

                if (IsOutline)
                {
                    return $"outline-{state}";
                }

                return state;
            }
        }

        protected virtual string HoverModeValue
        {
            get
            {
                if (HoverMode is ButtonHoverMode.Shadow)
                {
                    return "elevate-hover";
                }
                else if (HoverMode is ButtonHoverMode.Air)
                {
                    return "elevate-air";
                }
                else if (HoverMode is ButtonHoverMode.Outline)
                {
                    return $"outline-hover-{State.ToValue()}";
                }

                return "";
            }
        }

        protected virtual string Classes
        {
            get
            {
                ButtonState = IsDisabled ? ButtonState.Disabled : ButtonState;

                return CssClassBuilder.Build(
                    new CssClass("btn", true),
                    new CssClass("btn-icon", IsOnlyIcon),
                    new CssClass($"btn-icon-{OnlyIconSize.ToValue()}", IsOnlyIcon && OnlyIconSize.IsUsed()),
                    new CssClass("btn-upper", IsTextUpper),
                    new CssClass("btn-lower", IsTextUpper ? false : IsTextLower),
                    new CssClass($"btn-{Size.ToValue()}", Size.IsUsed()),
                    new CssClass($"btn-font-{FontSize.ToValue()}", FontSize.IsUsed()),
                    new CssClass($"btn-{FontWeight.ToValue()}", FontWeight.IsUsed()),
                    new CssClass($"btn-{Tall.ToValue()}", Tall.IsUsed()),
                    new CssClass($"btn-{Wide.ToValue()}", Wide.IsUsed()),
                    new CssClass($"btn-{ShapeValue}", Shape.IsUsed()),
                    new CssClass($"btn-{StateValue}", true),
                    new CssClass($"btn-elevate btn-{HoverModeValue}", HoverMode.IsUsed()),
                    new CssClass(ButtonState.ToValue(), ButtonState.IsUsed()),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var button = BuildButton(childContent);

            if (context.TryGetContext<ButtonDropdownContext, ButtonDropdownTagHelper>(out ButtonDropdownContext buttonDropdownContext))
            {
                buttonDropdownContext.ButtonOrDividers.Add(button);

                output.SuppressOutput();
            }
            else if (context.TryGetContext<ButtonGroupContext, ButtonGroupTagHelper>(out ButtonGroupContext buttonGroupContext))
            {
                buttonGroupContext.ButtonOrDropdowns.Add(button);

                output.SuppressOutput();
            }
            else
            {
                output.TransformOutput(button);
            }
        }

        protected virtual TagBuilder BuildButton(TagHelperContent childContent)
        {
            var button = new TagBuilder(KindValue);

            button.Attributes.Add("id", Id);

            button.Attributes.Add("class", Classes);

            if (KindValue == "button")
            {
                button.Attributes.Add("type", Kind.ToValue());
            }

            if (!String.IsNullOrWhiteSpace(Url))
            {
                button.Attributes.Add("href", Url);
            }

            if (ButtonState == ButtonState.Disabled)
            {
                button.Attributes.Add("disabled", "disabled");
            }

            if (Icon.IsUsed())
            {
                button.InnerHtml.AppendHtml(Icon.ToIconContent());
            }

            if (!IsOnlyIcon)
            {
                if (!String.IsNullOrWhiteSpace(Text))
                {
                    button.InnerHtml.AppendHtml(Text);
                }
                else
                {
                    button.InnerHtml.AppendHtml(childContent);
                }
            }

            return button;
        }
    }
}
