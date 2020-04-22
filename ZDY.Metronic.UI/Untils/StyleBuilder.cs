using System;
using System.Linq;

namespace ZDY.Metronic.UI.Untils
{
    internal static class StyleBuilder
    {
        internal static string Build(params ValueTuple<string, string, bool>[] styleItems)
        {
            var styles = styleItems.Where(t => t.Item3 == true);

            return styles.Count() > 0 ? String.Join(";", styles.Select(t => $"{t.Item1}:{t.Item2}")) : "";
        }
    }
}
