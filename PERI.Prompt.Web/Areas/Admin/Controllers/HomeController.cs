﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : BLL.BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("~/Admin/Setting")]
        public async Task<IActionResult> Setting()
        {
            using (var context = new EF.SampleDbContext())
            {
                ViewData["Title"] = "Setting";

                var res = await new BLL.Setting(context).Find(new EF.Setting());
                return View(res.OrderBy(x => x.Priority).ThenBy(x => x.Group).ToList());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/Admin/Setting")]
        public async Task<IActionResult> Setting(List<EF.Setting> args)
        {
            ViewData["Title"] = "Setting";

            var context = new EF.SampleDbContext();

            var bsetting = new BLL.Setting(context);

            foreach (var rec in args)
            {
                await bsetting.Edit(rec);
            }

            var res = await new BLL.Setting(context).Find(new EF.Setting());

            TempData["notice"] = "Succesfully updated settings.";

            return View(res.OrderBy(x => x.Priority).ThenBy(x => x.Group).ToList());
        }
    }
}
