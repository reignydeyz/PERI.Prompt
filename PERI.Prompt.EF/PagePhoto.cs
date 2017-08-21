using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class PagePhoto
    {
        public int PageId { get; set; }
        public int PhotoId { get; set; }

        public Page Page { get; set; }
        public Photo Photo { get; set; }
    }
}
