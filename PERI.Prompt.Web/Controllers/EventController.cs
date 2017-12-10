using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Prompt.Web.Controllers
{
    public class EventController : BLL.BaseTemplateController
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> View(int id)
        {
            var context = new EF.SampleDbContext();

            var rec = await new BLL.Event(context).Get(new EF.Event { EventId = id });

            return View(rec);
        }
    }
}