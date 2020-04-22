using System;
using System.Linq;

namespace ZDY.Metronic.UI.Untils
{
    internal static class CssClassBuilder
    {
        internal static string Build(params CssClass[] classNames)
        {
            var classes = classNames.Where(t => t.IsAppend == true);
            return classes.Count() > 0 ? String.Join(" ", classes.Select(t => t.ClassName)) : "";
        }
    }
}
