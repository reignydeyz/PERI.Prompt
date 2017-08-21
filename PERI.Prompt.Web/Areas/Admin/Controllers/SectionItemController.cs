using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class SectionItemController : BLL.BaseController
    {
        private readonly IHostingEnvironment _environment;
        public SectionItemController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        [Route("Section/{id:int}/Items")]
        public async Task<IActionResult> Index(int id)
        {
            var context = new EF.SampleDbContext();

            var section = await new BLL.Section(context).Get(new EF.Section { SectionId = id });

            ViewData["Title"] = "Section/" + section.Name;

            var tuple = new Tuple<EF.SectionItem, List<EF.SectionItem>>(new EF.SectionItem { SectionId = id }, section.SectionItem.ToList());
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Section/{id:int}/Items")]
        public async Task<IActionResult> Index([Bind(Prefix = "Item1")] EF.SectionItem args, int id)
        {
            var context = new EF.SampleDbContext();

            var section = await new BLL.Section(context).Get(new EF.Section { SectionId = id });

            ViewData["Title"] = "Section/" + section.Name;

            var items = await new BLL.SectionItem(context).Find(args);

            var tuple = new Tuple<EF.SectionItem, List<EF.SectionItem>>(args, items.ToList());

            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Section/{id:int}/Items/Save")]
        public async Task<IActionResult> Save([Bind(Prefix = "Item2")] List<EF.SectionItem> args, int id)
        {
            try
            {
                var context = new EF.SampleDbContext();
                foreach (var rec in args)
                    await new BLL.SectionItem(context).Edit(rec);
            }
            catch (DbUpdateException ex)
            {
                TempData["notice"] = "Entry is causing conflict or already exist.";
            }

            return RedirectToAction("Index", id);
        }

        [Route("Section/{id:int}/Items/New")]
        public async Task<IActionResult> New(int id)
        {
            var context = new EF.SampleDbContext();

            var section = await new BLL.Section(context).Get(new EF.Section { SectionId = id });
            ViewData["Title"] = "Section/" + section.Name + "/Items/New";

            var props = new List<EF.SectionItemProperty>();
            var list = await new BLL.SectionProperty(context).Find(new EF.SectionProperty { SectionId = id });

            foreach (var rec in list)
            {
                props.Add(new EF.SectionItemProperty { SectionPropertyId = rec.SectionPropertyId, SectionProperty = new EF.SectionProperty { Name = rec.Name } });
            }

            var tuple = new Tuple<EF.SectionItem, List<EF.SectionItemProperty>>(new EF.SectionItem(), props.ToList());

            return View(tuple);
        }

        /// <summary>
        /// New Section Item page submit
        /// </summary>
        /// <param name="args">SectionItem</param>
        /// <param name="props">SectionItemProperties</param>
        /// <param name="file">Photo</param>
        /// <param name="id">Section Id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Section/{id:int}/Items/New")]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.SectionItem args, [Bind(Prefix = "Item2")] List<EF.SectionItemProperty> props, IFormFile file, int id)
        {
            var context = new EF.SampleDbContext();

            // Add SectionItem
            args.CreatedBy = User.Identity.Name;
            args.DateCreated = DateTime.Now;
            args.ModifiedBy = args.CreatedBy;
            args.DateModified = args.DateCreated;
            args.SectionId = id;
            var siId = await new BLL.SectionItem(context).Add(args);
            
            IFormFile uploadedImage = file;
            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                var pid = await new BLL.Photo(context).Add(_environment, file);

                var siph = new EF.SectionItemPhoto();
                siph.PhotoId = pid;
                siph.SectionItemId = siId;
                await new BLL.SectionItemPhoto(context).Add(siph);
            }

            foreach (var rec in props)
            {
                if (rec.Value != null)
                {
                    // Add SectionItemProperty
                    var sipr = new EF.SectionItemProperty();
                    sipr.SectionItemId = siId;
                    sipr.SectionPropertyId = rec.SectionPropertyId;
                    sipr.Value = rec.Value;
                    await new BLL.SectionItemProperty(context).Add(sipr);
                }
            }

            return Redirect("~/Section/" + id + "/Items");
        }

        [Route("Section/{id:int}/Items/{id1:int}/Edit")]
        public async Task<IActionResult> Edit(int id, int id1)
        {
            var context = new EF.SampleDbContext();

            var si = await new BLL.SectionItem(context).Get(new EF.SectionItem { SectionItemId = id1 });
            ViewData["Title"] = "Section/" + si.Section.Name + "/Items/" + id1 + "/Edit";

            List<EF.SectionItemProperty> props = new List<EF.SectionItemProperty>();
            var list = await new BLL.SectionProperty(context).Find(new EF.SectionProperty { SectionId = id });
            foreach (var rec in list)
            {
                var sip = await new BLL.SectionItemProperty(context).Get(new EF.SectionItemProperty { SectionPropertyId = rec.SectionPropertyId, SectionItemId = id1 });
                if (sip != null)
                    props.Add(sip);
                else
                    props.Add(new EF.SectionItemProperty { SectionPropertyId = rec.SectionPropertyId, SectionProperty = new EF.SectionProperty { Name = rec.Name } });
            }

            var tuple = new Tuple<EF.SectionItem, List<EF.SectionItemProperty>>(si, props);

            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Section/{id:int}/Items/{id1:int}/Edit")]
        public async Task<IActionResult> Edit([Bind(Prefix = "Item1")] EF.SectionItem args, [Bind(Prefix = "Item2")] List<EF.SectionItemProperty> props, IFormFile file, int id, int id1)
        {
            var context = new EF.SampleDbContext();

            var si = new EF.SectionItem
            {
                SectionItemId = id1,
                SectionId = id,
                Title = args.Title,
                Body = args.Body,
                Order = args.Order,
                ModifiedBy = User.Identity.Name,
                DateModified = DateTime.Now
            };

            await new BLL.SectionItem(context).Edit(si);

            // Update Photo
            IFormFile uploadedImage = file;
            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                var bsectionitemphoto = new BLL.SectionItemPhoto(context);
                var bsectionitemphotos = await bsectionitemphoto.Find(new EF.SectionItemPhoto { SectionItemId = args.SectionItemId });
                if (bsectionitemphotos.Count() > 0)
                    await new BLL.Photo(context).Delete(bsectionitemphotos.First().PhotoId, _environment);

                var pid = await new BLL.Photo(context).Add(_environment, file);

                var sip = new EF.SectionItemPhoto();
                sip.SectionItemId = id1;
                sip.PhotoId = pid;
                await bsectionitemphoto.Edit(sip);
            }

            
            foreach (var rec in props)
            {
                if (rec.Value != null)
                {                    
                    if (rec.SectionItemId != 0)
                    {
                        // Update SectionItemProperty
                        var sipr = new EF.SectionItemProperty
                        {
                            SectionItemId = id1,
                            SectionPropertyId = rec.SectionPropertyId,
                            Value = rec.Value
                        };
                        await new BLL.SectionItemProperty(context).Edit(sipr);
                    }
                    else
                    {
                        // Add SectionItemProperty
                        var sipr = new EF.SectionItemProperty();
                        sipr.SectionItemId = args.SectionItemId;
                        sipr.SectionPropertyId = rec.SectionPropertyId;
                        sipr.Value = rec.Value;
                        await new BLL.SectionItemProperty(context).Add(sipr);
                    }
                }
                else
                {
                    // Delete SectionItemProperty
                    var bsip = new BLL.SectionItemProperty(context);

                    var sipr = await bsip.Get(new EF.SectionItemProperty { SectionItemId = args.SectionItemId, SectionPropertyId = rec.SectionPropertyId });

                    if (sipr != null)
                        await bsip.Delete(sipr);
                }
            }

            return Redirect("~/Section/" + id + "/Items");
        }

        [HttpPost]
        [Route("Section/{id:int}/Items/Delete")]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var context = new EF.SampleDbContext();

            var bsectionitem = new BLL.SectionItem(context);

            // Delete photos
            var siphotos = await new BLL.SectionItemPhoto(context).Get(ids);
            if (siphotos.Count() > 0)
                await new BLL.Photo(context).Delete(siphotos.Select(x => x.PhotoId).ToArray(), _environment);

            await bsectionitem.Delete(ids);

            return Json("Success!");
        }
    }
}
