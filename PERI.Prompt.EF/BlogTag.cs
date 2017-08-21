using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class BlogTag
    {
        public int BlogId { get; set; }
        public int TagId { get; set; }

        public Blog Blog { get; set; }
        public Tag Tag { get; set; }
    }
}
