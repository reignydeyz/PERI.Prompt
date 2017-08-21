using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class BlogPhoto
    {
        public int BlogId { get; set; }
        public int PhotoId { get; set; }

        public Blog Blog { get; set; }
        public Photo Photo { get; set; }
    }
}
