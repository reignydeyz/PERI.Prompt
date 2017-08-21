using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Section
    {
        public Section()
        {
            SectionItem = new HashSet<SectionItem>();
            SectionProperty = new HashSet<SectionProperty>();
        }

        public int SectionId { get; set; }
        public int TemplateId { get; set; }
        public string Name { get; set; }

        public Template Template { get; set; }
        public ICollection<SectionItem> SectionItem { get; set; }
        public ICollection<SectionProperty> SectionProperty { get; set; }
    }
}
