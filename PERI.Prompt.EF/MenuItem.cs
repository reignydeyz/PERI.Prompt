using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class MenuItem
    {
        public MenuItem()
        {
            ChildMenuItem = new HashSet<ChildMenuItem>();
        }

        public int MenuItemId { get; set; }
        public int MenuId { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }

        public Menu Menu { get; set; }
        public ICollection<ChildMenuItem> ChildMenuItem { get; set; }
    }
}
