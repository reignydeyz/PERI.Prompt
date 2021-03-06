﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PERI.Prompt.BLL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Main.Controllers
{
    [Area("Main")]
    [Authorize(Roles = "Admin,Blogger")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CategoryController : BLL.BaseController
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Categories";
            
            ViewBag.Data = await new BLL.Category(unitOfWork).Find(new EF.Category());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EF.Category args)
        {
            ViewData["Title"] = "Categories";
            ViewBag.Data = await new BLL.Category(unitOfWork).Find(args);
            return View();
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> New()
        {
            ViewData["Title"] = "Category/New";
            
            ViewBag.BlogSortOrders = (await new BLL.BlogSortOrder(unitOfWork).Find(new EF.BlogSortOrder())).ToList();

            var obj = new Tuple<EF.Category, bool>(new EF.Category(), true);
            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.Category model, [Bind(Prefix = "Item2")] bool isactive)
        {
            ViewData["Title"] = "Category/New";

            try
            {
                model.CreatedBy = User.Identity.Name;

                if (!isactive)
                    model.DateInactive = DateTime.Now;

                await new BLL.Category(unitOfWork).Add(model);
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

            ViewBag.BlogSortOrders = (await new BLL.BlogSortOrder(unitOfWork).Find(new EF.BlogSortOrder())).ToList();

            var obj = new Tuple<EF.Category, bool>(model, isactive);

            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Category/Edit";

            ViewBag.BlogSortOrders = (await new BLL.BlogSortOrder(unitOfWork).Find(new EF.BlogSortOrder())).ToList();

            var obj = await new BLL.Category(unitOfWork).GetModel(id);
            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind(Prefix = "Item1")] EF.Category model, [Bind(Prefix = "Item2")] bool isactive)
        {
            ViewData["Title"] = "Category/Edit";
            
            try
            {
                model.ModifiedBy = User.Identity.Name;

                if (!isactive)
                    model.DateInactive = DateTime.Now;

                await new BLL.Category(unitOfWork).Edit(model);
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

            ViewBag.BlogSortOrders = (await new BLL.BlogSortOrder(unitOfWork).Find(new EF.BlogSortOrder())).ToList();

            var obj = new Tuple<EF.Category, bool>(model, isactive);

            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bcategory = new BLL.Category(unitOfWork);            

            await bcategory.Delete(ids);

            return Json("Success!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Activate([FromBody] int[] ids)
        {
            var bcategory = new BLL.Category(unitOfWork);

            await bcategory.Activate(ids);

            return Json("Success!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Deactivate([FromBody] int[] ids)
        {
            var bcategory = new BLL.Category(unitOfWork);

            await bcategory.Deactivate(ids);

            return Json("Success!");
        }
    }
}
