using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.Metronic.UI.TagHelpers
{
    public class SelectBoxContext : ITagHelperContext
    {
        public List<SelectOption> Options { get; set; } = new List<SelectOption>();
    }
}
