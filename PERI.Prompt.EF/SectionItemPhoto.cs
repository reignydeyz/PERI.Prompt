using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class SectionItemPhoto
    {
        public int SectionItemId { get; set; }
        public int PhotoId { get; set; }

        public Photo Photo { get; set; }
        public SectionItem SectionItem { get; set; }
    }
}
