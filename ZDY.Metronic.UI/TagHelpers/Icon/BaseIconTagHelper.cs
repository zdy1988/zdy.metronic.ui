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
    public abstract class BaseIconTagHelper<TIcon> : HelperBase
    {
        public virtual TIcon Name { get; set; }

        protected abstract string GetIcon();

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass($"{GetIcon()}", true),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "i";

            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", Id);

            output.Attributes.Add("class", Classes);
        }
    }
}
