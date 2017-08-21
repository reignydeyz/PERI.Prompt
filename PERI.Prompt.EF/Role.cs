using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }

        public ICollection<User> User { get; set; }
    }
}
