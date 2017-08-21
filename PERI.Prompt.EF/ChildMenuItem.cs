using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class ChildMenuItem
    {
        public int ChildMenuItemId { get; set; }
        public int MenuItemId { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }

        public MenuItem MenuItem { get; set; }
    }
}
