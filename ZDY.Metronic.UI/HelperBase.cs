using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ZDY.Metronic.UI
{
    public class HelperBase : TagHelper
    {
        protected readonly string Key = Guid.NewGuid().ToString();

        private string id;

        public virtual string Id
        {
            get
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    var name = this.GetType().Name.Replace("TagHelper", "").ParseValue().ToLower();

                    return $"{name}-{Key}";
                }

                return id;
            }
            set
            {
                id = value;
            }
        }

        public virtual string ClassNames { get; set; }

        protected string Use(IHtmlContent htmlContent)
        {
            return htmlContent == null ? "" : htmlContent.ToHtml();
        }
    }

    public class HelperBase<TIcon> : HelperBase
        where TIcon : struct
    {
        public virtual TIcon Icon { get; set; }
    }
}
