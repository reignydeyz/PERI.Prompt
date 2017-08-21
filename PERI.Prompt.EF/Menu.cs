using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Menu
    {
        public Menu()
        {
            MenuItem = new HashSet<MenuItem>();
        }

        public int MenuId { get; set; }
        public string Name { get; set; }

        public ICollection<MenuItem> MenuItem { get; set; }
    }
}
