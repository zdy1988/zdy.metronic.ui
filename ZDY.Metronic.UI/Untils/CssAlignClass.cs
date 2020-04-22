using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.Metronic.UI.Untils
{
    internal class AlignClass : CssClass
    {
        internal AlignClass(Align align)
            : base($"kt-align-{align.ToValue()}", align.IsUsed())
        {

        }
    }
}
