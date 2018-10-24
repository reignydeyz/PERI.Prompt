using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PERI.Prompt.BLL;

namespace PERI.Prompt.Web.Areas.Main.Controllers
{
    [Area("Main")]
    [Authorize(Roles = "Admin,Blogger")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EventController : BLL.BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment _environment;
        public EventController(IHostingEnvironment environment, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Events";

            ViewBag.Data = await new BLL.Event(unitOfWork).Find(new EF.Event());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EF.Event args)
        {
            ViewData["Title"] = "Events";
            ViewBag.Data = await new BLL.Event(unitOfWork).Find(args);
            return View();
        }

        public IActionResult New()
        {
            ViewData["Title"] = "Event/New";
            
            return View(new Tuple<EF.Event, bool>(new EF.Event { Time = DateTime.Now }, true));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.Event model, [Bind(Prefix = "Item2")] bool isactive, IFormFile file)
        {
            ViewData["Title"] = "Event/New";

            model.CreatedBy = User.Identity.Name;

            var bevent = new BLL.Event(unitOfWork);

            // Add Event
            if (!isactive)
                model.DateInactive = DateTime.Now;
            else
                model.DateInactive = null;

            var id = await bevent.Add(model);

            // Add Photo
            IFormFile uploadedImage = file;
            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                var pid = await new BLL.Photo(unitOfWork).Add(_environment, file);

                var ep = new EF.EventPhoto();
                ep.EventId = id;
                ep.PhotoId = pid;
                await new BLL.EventPhoto(unitOfWork).Add(ep);
            }

            return Redirect("~/Main/Event");
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Event/Edit";

            var obj = await (new BLL.Event(unitOfWork).GetModel(id));
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind(Prefix = "Item1")] EF.Event model, [Bind(Prefix = "Item2")] bool isactive, IFormFile file)
        {
            ViewData["Title"] = "Event/Edit";

            model.ModifiedBy = User.Identity.Name;

            var bevent = new BLL.Event(unitOfWork);

            // Update Event
            if (!isactive)
                model.DateInactive = DateTime.Now;
            else
                model.DateInactive = null;

            await bevent.Edit(model);

            // Update Photo
            IFormFile uploadedImage = file;
            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                var beventphoto = new BLL.EventPhoto(unitOfWork);
                var eventphotos = await beventphoto.Find(new EF.EventPhoto { EventId = model.EventId });
                if (eventphotos.Count() > 0)
                    await new BLL.Photo(unitOfWork).Delete(eventphotos.First().PhotoId, _environment);

                var pid = await new BLL.Photo(unitOfWork).Add(_environment, file);

                var ep = new EF.EventPhoto();
                ep.EventId = model.EventId;
                ep.PhotoId = pid;
                await beventphoto.Edit(ep);
            }

            return Redirect("~/Main/Event");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bevent = new BLL.Event(unitOfWork);

            // Delete photos
            var eventphotos = await new BLL.EventPhoto(unitOfWork).Get(ids);
            if (eventphotos.Count() > 0)
                await new BLL.Photo(unitOfWork).Delete(eventphotos.Select(x => x.PhotoId).ToArray(), _environment);

            await bevent.Delete(ids);

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Activate([FromBody] int[] ids)
        {
            var bevent = new BLL.Event(unitOfWork);

            await bevent.Activate(ids);

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate([FromBody] int[] ids)
        {
            var bevent = new BLL.Event(unitOfWork);

            await bevent.Deactivate(ids);

            return Json("Success!");
        }
    }
}