using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Main.Controllers
{
    [Area("Main")]
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CategoryController : BLL.BaseController
    {
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Categories";

            var context = new EF.SampleDbContext();

            ViewBag.Data = await new BLL.Category(context).Find(new EF.Category());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EF.Category args)
        {
            ViewData["Title"] = "Categories";
            var context = new EF.SampleDbContext();
            ViewBag.Data = await new BLL.Category(context).Find(args);
            return View();
        }
    }
}
