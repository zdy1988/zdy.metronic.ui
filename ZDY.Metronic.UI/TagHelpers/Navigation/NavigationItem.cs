using System;
using System.Collections.Generic;

namespace ZDY.Metronic.UI
{
    public class NavigationItem
    {
        public string Text { get; set; }

        public Flat2Icon Icon { get; set; } 

        public string Section { get; set; }

        public string Badge { get; set; }

        public State BadgeState { get; set; } = State.Brand;

        public List<NavigationItem> Navs { get; set; } = new List<NavigationItem>();

        public bool IsActived { get; set; } = false;

        public int Order { get; set; } = 0;
    }
}
