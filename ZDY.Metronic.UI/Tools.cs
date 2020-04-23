using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.Metronic.UI
{
    public static class Tools
    {
        public static string GetIconContent(object icon)
        {
            return icon.ToIconContent();
        }
    }
}
