using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PERI.Prompt.BLL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class SectionController : BLL.BaseController
    {
        private readonly IUnitOfWork unitOfWork;

        public SectionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var context = new EF.SampleDbContext();

            var template = (await new BLL.Template(unitOfWork).Find(new EF.Template())).Where(x => x.DateInactive == null).First();

            ViewData["Title"] = "Sections";
            ViewBag.Data = await new BLL.Section(unitOfWork).Find(new EF.Section { TemplateId = template.TemplateId });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EF.Section args)
        {
            var template = (await new BLL.Template(unitOfWork).Find(new EF.Template())).Where(x => x.DateInactive == null).First();
            args.TemplateId = template.TemplateId;

            ViewData["Title"] = "Sections";
            ViewBag.Data = await new BLL.Section(unitOfWork).Find(args);

            return View();
        }
    }
}
