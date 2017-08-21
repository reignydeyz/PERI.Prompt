using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Page
    {
        public Page()
        {
            PagePhoto = new HashSet<PagePhoto>();
        }

        public int PageId { get; set; }
        public string Title { get; set; }
        public string Permalink { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateInactive { get; set; }

        public ICollection<PagePhoto> PagePhoto { get; set; }
    }
}
