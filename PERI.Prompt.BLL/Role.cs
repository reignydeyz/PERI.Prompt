using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Prompt.BLL
{
    public class Role
    {
        EF.SampleDbContext context;

        public Role(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        /// <summary>
        /// Creates SelectList of Roles
        /// </summary>
        /// <returns></returns>
        public SelectList DropDown(string selectedVal = null)
        {
            // Add context and select records
            
            List<Core.DropDownList.Item> ddItem = new List<Core.DropDownList.Item>();

            var qry = context.Role;

            foreach (var obj in qry)
                ddItem.Add(new Core.DropDownList.Item(obj.Name, obj.RoleId.ToString()));

            return new SelectList(ddItem, "Value", "Text", selectedVal);
        }
    }
}
