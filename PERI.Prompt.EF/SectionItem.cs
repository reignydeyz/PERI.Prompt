using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class SectionItem
    {
        public SectionItem()
        {
            SectionItemPhoto = new HashSet<SectionItemPhoto>();
            SectionItemProperty = new HashSet<SectionItemProperty>();
        }

        public int SectionItemId { get; set; }
        public int SectionId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Order { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateInactive { get; set; }

        public Section Section { get; set; }
        public ICollection<SectionItemPhoto> SectionItemPhoto { get; set; }
        public ICollection<SectionItemProperty> SectionItemProperty { get; set; }
    }
}
