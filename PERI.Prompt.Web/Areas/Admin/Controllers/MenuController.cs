using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class MenuController : BLL.BaseController
    {
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var context = new EF.SampleDbContext();

            ViewData["Title"] = "Menus";
            ViewBag.Data = await new BLL.Menu(context).Find(new EF.Menu ());

            return View();
        }
    }
}
