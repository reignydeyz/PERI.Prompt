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
        private readonly IUnitOfWork unitOfWork;

        public Role(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates SelectList of Roles
        /// </summary>
        /// <returns></returns>
        public SelectList DropDown(string selectedVal = null)
        {
            // Add context and select records
            
            List<Core.DropDownList.Item> ddItem = new List<Core.DropDownList.Item>();

            var qry = unitOfWork.RoleRepository.Entities;

            foreach (var obj in qry)
                ddItem.Add(new Core.DropDownList.Item(obj.Name, obj.RoleId.ToString()));

            return new SelectList(ddItem, "Value", "Text", selectedVal);
        }

        public async Task<EF.Role> GetById(int id)
        {
            return await unitOfWork.RoleRepository.Entities.FirstAsync(x => x.RoleId == id);
        }
    }
}
