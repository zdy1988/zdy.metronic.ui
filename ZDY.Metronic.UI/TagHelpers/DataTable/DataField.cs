using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.Metronic.UI
{
    public class DataField
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public bool IsSort { get; set; }

        public bool IsCenter { get; set; }

        public int Width { get; set; }

        public bool IsAutoWidth { get; set; } = false;

        public bool IsAutoHide { get; set; } = true;
    }
}
