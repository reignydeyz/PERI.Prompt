using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Blog
    {
        public Blog()
        {
            BlogPhoto = new HashSet<BlogPhoto>();
            BlogTag = new HashSet<BlogTag>();
        }

        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DatePublished { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateInactive { get; set; }

        public ICollection<BlogPhoto> BlogPhoto { get; set; }
        public ICollection<BlogTag> BlogTag { get; set; }
    }
}
