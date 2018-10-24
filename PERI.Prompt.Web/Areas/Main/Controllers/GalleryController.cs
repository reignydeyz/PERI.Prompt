using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using PERI.Prompt.BLL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Admin.Controllers
{
    [Area("Main")]
    [Authorize(Roles = "Admin,Blogger")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class GalleryController : BLL.BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment _environment;
        public GalleryController(IHostingEnvironment environment, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _environment = environment;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Galleries";

            ViewBag.Data = await new BLL.Gallery(unitOfWork).Find(new EF.Gallery());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EF.Gallery args)
        {
            ViewData["Title"] = "Galleries";

            ViewBag.Data = await new BLL.Gallery(unitOfWork).Find(args);
            return View();
        }
    }
}
