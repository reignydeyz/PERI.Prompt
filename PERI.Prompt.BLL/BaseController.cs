using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PERI.Prompt.BLL
{
    /// <summary>
    /// <see cref="https://coderwall.com/p/cibprg/basecontroller-in-asp-net-mvc"/>
    /// <seealso cref="https://stackoverflow.com/questions/27308524/access-viewbag-property-on-all-views"/>
    /// </summary>
    [HandleException]
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (var context = new EF.SampleDbContext())
            {
                ViewBag.Settings = context.Setting.ToList();
                base.OnActionExecuting(filterContext);
            }                
        }
    }
}
