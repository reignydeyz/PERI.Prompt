using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class SectionItemProperty
    {
        public int SectionPropertyId { get; set; }
        public int SectionItemId { get; set; }
        public string Value { get; set; }

        public SectionItem SectionItem { get; set; }
        public SectionProperty SectionProperty { get; set; }
    }
}
