using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Visibility
    {
        public Visibility()
        {
            Blog = new HashSet<Blog>();
        }

        public int VisibilityId { get; set; }
        public string Name { get; set; }

        public ICollection<Blog> Blog { get; set; }
    }
}
