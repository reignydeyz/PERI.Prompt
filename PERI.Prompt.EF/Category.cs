using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Category
    {
        public Category()
        {
            BlogCategory = new HashSet<BlogCategory>();
        }

        public int CategoryId { get; set; }
        public int BlogSortOrderId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateInactive { get; set; }

        public BlogSortOrder BlogSortOrder { get; set; }
        public ICollection<BlogCategory> BlogCategory { get; set; }
    }
}
