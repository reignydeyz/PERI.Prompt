using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class MenuItemController : BLL.BaseController
    {
        // GET: /<controller>/
        [Route("Menu/{id:int}/Items")]
        public async Task<IActionResult> Index(int id)
        {
            var context = new EF.SampleDbContext();

            var menu = await new BLL.Menu(context).Get(new EF.Menu { MenuId = id });

            ViewData["Title"] = "Menu/" + menu.Name;

            var tuple = new Tuple<EF.MenuItem, List<EF.MenuItem>>(new EF.MenuItem { MenuId = id }, menu.MenuItem.ToList());

            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Menu/{id:int}/Items/New")]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.MenuItem args, int id)
        {
            try
            {
                await new BLL.MenuItem(new EF.SampleDbContext()).Add(args);
            }
            catch (DbUpdateException ex)
            {
                TempData["notice"] = "Entry is causing conflict or already exist.";
            }

            return RedirectToAction("Index", id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Menu/{id:int}/Items/Save")]
        public async Task<IActionResult> Save([Bind(Prefix = "Item2")] List<EF.MenuItem> args, int id)
        {
            try
            {
                var context = new EF.SampleDbContext();
                foreach (var rec in args)
                    await new BLL.MenuItem(context).Edit(rec);
            }
            catch (DbUpdateException ex)
            {
                TempData["notice"] = "Entry is causing conflict or already exist.";
            }

            return RedirectToAction("Index", id);
        }

        [HttpPost]
        [Route("Menu/{id:int}/Items/Delete")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var context = new EF.SampleDbContext();

            await new BLL.MenuItem(context).Delete(ids);

            return Json("Success!");
        }
    }
}
