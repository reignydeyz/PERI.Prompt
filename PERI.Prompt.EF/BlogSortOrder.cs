using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class BlogSortOrder
    {
        public BlogSortOrder()
        {
            Category = new HashSet<Category>();
        }

        public int BlogSortOrderId { get; set; }
        public string Name { get; set; }

        public ICollection<Category> Category { get; set; }
    }
}
