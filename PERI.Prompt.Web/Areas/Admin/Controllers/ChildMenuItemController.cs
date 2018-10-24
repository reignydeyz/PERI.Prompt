using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PERI.Prompt.BLL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChildMenuItemController : BLL.BaseController
    {
        private readonly IUnitOfWork unitOfWork;

        public ChildMenuItemController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        [Route("Menu/{id:int}/Items/{id1:int}/Children")]
        public async Task<IActionResult> Index(int id, int id1)
        {
            var menuitem = await new BLL.MenuItem(unitOfWork).Get(new EF.MenuItem { MenuItemId = id1 });

            ViewData["Title"] = "Menu/" + menuitem.Menu.Name + "/" + menuitem.Label;

            var tuple = new Tuple<EF.ChildMenuItem, List<EF.ChildMenuItem>>(new EF.ChildMenuItem { MenuItem = menuitem, MenuItemId = id1 }, menuitem.ChildMenuItem.ToList());

            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Menu/{id:int}/Items/{id1:int}/Children/New")]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.ChildMenuItem args, int id, int id1)
        {
            try
            {
                await new BLL.ChildMenuItem(unitOfWork).Add(args);
            }
            catch (DbUpdateException ex)
            {
                TempData["notice"] = "Entry is causing conflict or already exist.";
            }

            return RedirectToAction("Index", new { id = id, id1 = id1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Menu/{id:int}/Items/{id1:int}/Children/Save")]
        public async Task<IActionResult> Save([Bind(Prefix = "Item2")] List<EF.ChildMenuItem> args, int id, int id1)
        {
            try
            {
                foreach (var rec in args)
                    await new BLL.ChildMenuItem(unitOfWork).Edit(rec);
            }
            catch (DbUpdateException ex)
            {
                TempData["notice"] = "Entry is causing conflict or already exist.";
            }

            return RedirectToAction("Index", new { id = id, id1 = id1 });
        }

        [HttpPost]
        [Route("Menu/{id:int}/Items/{id1:int}/Children/Delete")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            await new BLL.ChildMenuItem(unitOfWork).Delete(ids);

            return Json("Success!");
        }
    }
}
