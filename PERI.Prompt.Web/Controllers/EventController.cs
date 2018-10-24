using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PERI.Prompt.BLL;

namespace PERI.Prompt.Web.Controllers
{
    public class EventController : BLL.BaseTemplateController
    {
        private readonly IUnitOfWork unitOfWork;

        public EventController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> View(int id)
        {
            var rec = await new BLL.Event(unitOfWork).Get(new EF.Event { EventId = id });

            ViewData["Title"] = rec.Name;

            return View(rec);
        }
    }
}