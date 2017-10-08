using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PERI.Prompt.BLL
{
    [HandleException]
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

        public async Task<EF.Role> GetById(int id)
        {
            return await context.Role.FirstAsync(x => x.RoleId == id);
        }
    }
}
