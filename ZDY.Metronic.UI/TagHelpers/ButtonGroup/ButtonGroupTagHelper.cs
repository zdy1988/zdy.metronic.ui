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
    [HtmlTargetElement("button-group")]
    public class ButtonGroupTagHelper : BaseTagHelper
    {
        public virtual Size Size { get; set; } = Size.None;

        public virtual bool IsVertical { get; set; } = false;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                   new CssClass("btn-group", !IsVertical),
                   new CssClass("btn-group-vertical", IsVertical),
                   new CssClass($"btn-group-{Size.ToValue()}", Size.IsUsed()),
                   new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<ButtonGroupContext, ButtonGroupTagHelper>(out ButtonGroupContext buttonGroupContext))
            {
                await output.GetChildContentAsync();

                if (buttonGroupContext.ButtonOrDropdowns.Any() || buttonGroupContext.ButtonOrDropdowns.Any())
                {
                    var buttonGroup = new TagBuilder("div");

                    buttonGroup.Attributes.Add("id", Id);

                    buttonGroup.Attributes.Add("class", Classes);

                    buttonGroup.Attributes.Add("role", "group");

                    foreach (var child in buttonGroupContext.ButtonOrDropdowns)
                    {
                        buttonGroup.InnerHtml.AppendHtml(child.ToHtml());
                    }

                    if (context.TryGetContext<ButtonToolbarContext, ButtonToolbarTagHelper>(out ButtonToolbarContext buttonToolbarContext))
                    {
                        buttonToolbarContext.Groups.Add(buttonGroup);
                    }
                    else
                    {
                        output.TransformOutput(buttonGroup);
                    }

                    return;
                }
            }

            output.SuppressOutput();
        }
    }
}
