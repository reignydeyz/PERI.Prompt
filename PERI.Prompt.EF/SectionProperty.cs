using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class SectionProperty
    {
        public SectionProperty()
        {
            SectionItemProperty = new HashSet<SectionItemProperty>();
        }

        public int SectionPropertyId { get; set; }
        public int SectionId { get; set; }
        public string Name { get; set; }

        public Section Section { get; set; }
        public ICollection<SectionItemProperty> SectionItemProperty { get; set; }
    }
}
