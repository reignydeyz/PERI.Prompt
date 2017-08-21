using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Tag
    {
        public Tag()
        {
            BlogTag = new HashSet<BlogTag>();
        }

        public int TagId { get; set; }
        public string Name { get; set; }

        public ICollection<BlogTag> BlogTag { get; set; }
    }
}
