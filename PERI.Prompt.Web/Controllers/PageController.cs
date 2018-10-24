using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PERI.Prompt.BLL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Controllers
{
    public class PageController : BLL.BaseTemplateController
    {
        private readonly IUnitOfWork unitOfWork;

        public PageController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(string permalink)
        {
            var context = new EF.SampleDbContext();

            var rec = await new BLL.Page(unitOfWork).Get(new EF.Page { Permalink = permalink });

            if (rec != null)
            {
                ViewData["Title"] = rec.Title;
                return View(rec);
            }
            else
                return StatusCode(404);
        }
    }
}
