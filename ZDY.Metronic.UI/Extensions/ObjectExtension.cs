using System;

namespace ZDY.Metronic.UI
{
    internal static class ObjectExtension
    {
        internal static bool IsNull(this object obj)
        {
            return obj == null;
        }

        internal static bool IsNotNull(this object obj)
        {
            return !obj.IsNull();
        }
    }
}
