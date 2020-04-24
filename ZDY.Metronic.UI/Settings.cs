using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.Metronic.UI
{
    public class Settings
    {
        private static readonly object locker = new object();

        private static Settings instance;

        internal static Settings GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new Settings();
                    }
                }
            }
            return instance;
        }

        public bool IsFormBoxHelpTextDestroyed { get; set; } = false;

        public string TextBoxPlaceholderFormat { get; set; } = $"Please Enter {0}...";

        public string TextBoxHelpTextFormat { get; set; } = $"Please Enter {0}...";

        public string CheckBoxHelpTextFormat { get; set; } = $"Please Select {0}...";

        public string SelectBoxPlaceholderFormat { get; set; } = $"Please Select {0}...";

        public string SelectBoxHelpTextFormat { get; set; } = $"Please Select {0}...";
    }
}
