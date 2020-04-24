using System;
using System.Linq;

namespace ZDY.Metronic.UI
{
    internal static class StringBuilder
    {
        internal static string Build(params ValueTuple<string, bool>[] inputs)
        {
            var strings = inputs.Where(t => t.Item2 == true);

            return strings.Count() > 0 ? String.Join(",", strings) : "";
        }
    }
}
