using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.Metronic.UI
{
    public class MetronicUI : IMetronicUI
    {
        public string GetIconContent(object icon)
        {
            return icon.ToIconContent();
        }
    }
}
