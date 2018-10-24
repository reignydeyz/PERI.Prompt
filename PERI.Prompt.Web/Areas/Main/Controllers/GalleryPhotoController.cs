using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using PERI.Prompt.BLL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Main.Controllers
{
    [Area("Main")]
    [Authorize(Roles = "Admin,Blogger")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class GalleryPhotoController : BLL.BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment _environment;
        public GalleryPhotoController(IHostingEnvironment environment, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _environment = environment;
        }

        [Route("Gallery/{id:int}/Photos")]
        public async Task<IActionResult> Index(int id)
        {
            var gallery = await new BLL.Gallery(unitOfWork).Get(new EF.Gallery { GalleryId = id });
            ViewData["Title"] = "Gallery/" + gallery.Name;
            ViewBag.Data = gallery.GalleryPhoto;
            return View(new EF.GalleryPhoto { GalleryId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Gallery/{id:int}/Photos")]
        public async Task<IActionResult> Index(EF.GalleryPhoto args)
        {
            var gallery = await new BLL.Gallery(unitOfWork).Get(new EF.Gallery { GalleryId = args.GalleryId });
            ViewData["Title"] = "Gallery/" + gallery.Name;
            ViewBag.Data = await new BLL.GalleryPhoto(unitOfWork).Find(args);
            return View(new EF.GalleryPhoto { GalleryId = args.GalleryId });
        }

        [Route("Gallery/{id:int}/Photos/New")]
        public async Task<IActionResult> New(int id)
        {
            var gallery = await new BLL.Gallery(unitOfWork).Get(new EF.Gallery { GalleryId = id });
            ViewData["Title"] = "Gallery/" + gallery.Name + "/New";
            return View(new EF.GalleryPhoto { GalleryId  = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Gallery/{id:int}/Photos/New")]
        public async Task<IActionResult> New(EF.GalleryPhoto args, IFormFile file)
        {
            IFormFile uploadedImage = file;
            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                // Add photo
                var pid = await new BLL.Photo(unitOfWork).Add(_environment, file);

                // Add GalleryPhoto
                await new BLL.GalleryPhoto(unitOfWork).Add(new EF.GalleryPhoto {
                    GalleryId = args.GalleryId,
                    PhotoId = pid,
                    Title = args.Title,
                    Description = args.Description,
                    DateCreated = DateTime.Now,
                    CreatedBy = User.Identity.Name,
                    ModifiedBy = User.Identity.Name,
                    DateModified = DateTime.Now
                });

                return Redirect("~/Gallery/" + args.GalleryId + "/Photos/");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No image to upload");
                return View(args);
            }
        }

        /// <summary>
        /// Deletes specific GelleryPhoto
        /// </summary>
        /// <param name="id">Galleryid</param>
        /// <param name="id2">Photoid</param>
        /// <returns></returns>
        [Route("Gallery/{id:int}/Photos/{id1:int}/Delete")]
        public async Task<IActionResult> Delete(int id, int id1)
        {
            // Delete GalleryPhoto
            await new BLL.GalleryPhoto(unitOfWork).Delete(new EF.GalleryPhoto { GalleryId = id, PhotoId = id1 });
            
            // Delete photo
            await new BLL.Photo(unitOfWork).Delete(id1, _environment);

            return Redirect("~/Gallery/" + id + "/Photos");
        }

        [Route("Gallery/{id:int}/Photos/{id1:int}/Edit")]
        public async Task<IActionResult> Edit(int id, int id1)
        {
            var rec = await new BLL.GalleryPhoto(unitOfWork).Get(new EF.GalleryPhoto { GalleryId = id, PhotoId = id1 });
            ViewData["Title"] = "Gallery/" + rec.Gallery.Name + "/Photo/" + rec.Title + "/Edit";
            return View(rec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Gallery/{id:int}/Photos/{id1:int}/Edit")]
        public async Task<IActionResult> Edit(EF.GalleryPhoto args, IFormFile file)
        {
            // Update Photo
            IFormFile uploadedImage = file;
            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                // Delete GalleryPhoto
                await new BLL.GalleryPhoto(unitOfWork).Delete(new EF.GalleryPhoto { GalleryId = args.GalleryId, PhotoId = args.PhotoId });

                // Delete photo
                await new BLL.Photo(unitOfWork).Delete(args.PhotoId, _environment);

                // Add photo
                var pid = await new BLL.Photo(unitOfWork).Add(_environment, file);

                // Add GalleryPhoto
                await new BLL.GalleryPhoto(unitOfWork).Add(new EF.GalleryPhoto
                {
                    GalleryId = args.GalleryId,
                    PhotoId = pid,
                    Title = args.Title,
                    Description = args.Description,
                    DateCreated = DateTime.Now,
                    CreatedBy = User.Identity.Name,
                    ModifiedBy = User.Identity.Name,
                    DateModified = DateTime.Now
                });

                return Redirect("~/Gallery/" + args.GalleryId + "/Photos/");
            }

            args.ModifiedBy = User.Identity.Name;
            args.DateModified = DateTime.Now;
            await new BLL.GalleryPhoto(unitOfWork).Edit(args);

            return Redirect("~/Gallery/" + args.GalleryId + "/Photos/");
        }
    }
}
