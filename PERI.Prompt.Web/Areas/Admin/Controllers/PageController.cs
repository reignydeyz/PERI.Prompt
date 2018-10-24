using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PERI.Prompt.BLL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PageController : BLL.BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment _environment;
        public PageController(IUnitOfWork unitOfWork, IHostingEnvironment environment)
        {
            this.unitOfWork = unitOfWork;
            _environment = environment;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Pages";
            ViewBag.Data = await new BLL.Page(unitOfWork).Find(new EF.Page());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EF.Page args)
        {
            ViewData["Title"] = "Pages";
            ViewBag.Data = await new BLL.Page(unitOfWork).Find(args);
            return View();
        }

        public IActionResult New()
        {
            ViewData["Title"] = "Page/New";
            var obj = new Tuple<EF.Page, bool>(new EF.Page(), true);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.Page model, [Bind(Prefix = "Item2")] bool isactive, IFormFile file)
        {
            ViewData["Title"] = "Blog/New";

            try
            {
                model.CreatedBy = User.Identity.Name;

                var bpage = new BLL.Page(unitOfWork);

                // Add Page
                if (!isactive)
                    model.DateInactive = DateTime.Now;

                var id = await bpage.Add(model);

                // Add Photo
                IFormFile uploadedImage = file;
                if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
                {
                    var pid = await new BLL.Photo(unitOfWork).Add(_environment, file);

                    var pp = new EF.PagePhoto();
                    pp.PageId = id;
                    pp.PhotoId = pid;
                    await new BLL.PagePhoto(unitOfWork).Add(pp);
                }

                return Redirect("~/Admin/Page");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Entry is causing conflict or already exist.");
                return View(new Tuple<EF.Page, bool>(model, isactive));
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Page/Edit";
            var obj = await new BLL.Page(unitOfWork).GetModel(id);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind(Prefix = "Item1")] EF.Page model, [Bind(Prefix = "Item2")] bool isactive, IFormFile file)
        {
            ViewData["Title"] = "Page/Edit";

            try
            {
                model.ModifiedBy = User.Identity.Name;

                // Update Page
                if (!isactive)
                    model.DateInactive = DateTime.Now;

                await new BLL.Page(unitOfWork).Edit(model);

                // Update Photo
                IFormFile uploadedImage = file;
                if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
                {
                    var bpagephoto = new BLL.PagePhoto(unitOfWork);
                    var pagephotos = await bpagephoto.Find(new EF.PagePhoto { PageId = model.PageId });
                    if (pagephotos.Count() > 0)
                        await new BLL.Photo(unitOfWork).Delete(pagephotos.First().PhotoId, _environment);

                    var pid = await new BLL.Photo(unitOfWork).Add(_environment, file);

                    var pp = new EF.PagePhoto();
                    pp.PageId = model.PageId;
                    pp.PhotoId = pid;
                    await new BLL.PagePhoto(unitOfWork).Edit(pp);
                }

                return Redirect("~/Admin/Page");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Entry is causing conflict or already exist.");
                return View(new Tuple<EF.Page, bool>(model, isactive));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bpage = new BLL.Page(unitOfWork);

            // Delete photos
            var pagephotos = await new BLL.PagePhoto(unitOfWork).Get(ids);
            if (pagephotos.Count() > 0)
                await new BLL.Photo(unitOfWork).Delete(pagephotos.Select(x => x.PhotoId).ToArray(), _environment);

            await bpage.Delete(ids);

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Activate([FromBody] int[] ids)
        {
            var bpage = new BLL.Page(unitOfWork);

            await bpage.Activate(ids);

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate([FromBody] int[] ids)
        {
            var bpage = new BLL.Page(unitOfWork);

            await bpage.Deactivate(ids);

            return Json("Success!");
        }
    }
}
