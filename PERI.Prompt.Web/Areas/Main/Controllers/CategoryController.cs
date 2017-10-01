using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Main.Controllers
{
    [Area("Main")]
    [Authorize(Roles = "Admin,Blogger")]
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

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> New()
        {
            ViewData["Title"] = "Category/New";

            var context = new EF.SampleDbContext();

            ViewBag.BlogSortOrders = (await new BLL.BlogSortOrder(new EF.SampleDbContext()).Find(new EF.BlogSortOrder())).ToList();

            var obj = new Tuple<EF.Category, bool>(new EF.Category(), true);
            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.Category model, [Bind(Prefix = "Item2")] bool isactive)
        {
            ViewData["Title"] = "Category/New";

            var context = new EF.SampleDbContext();

            try
            {
                model.CreatedBy = User.Identity.Name;
                await new BLL.Category(context).Add(model);
                return Redirect("~/Main/Category");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Entry is causing conflict to other records.");
            }
            catch (Exception ex)
            { 
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            ViewBag.BlogSortOrders = (await new BLL.BlogSortOrder(new EF.SampleDbContext()).Find(new EF.BlogSortOrder())).ToList();

            var obj = new Tuple<EF.Category, bool>(model, isactive);

            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Category/Edit";

            var context = new EF.SampleDbContext();

            ViewBag.BlogSortOrders = (await new BLL.BlogSortOrder(new EF.SampleDbContext()).Find(new EF.BlogSortOrder())).ToList();

            var obj = await new BLL.Category(context).GetModel(id);
            return View(obj);
        }
    }
}
