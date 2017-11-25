using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Controllers
{
    public class SearchController : BLL.BaseTemplateController
    {
        // GET: /<controller>/
        [Route("Search/{qry}")]
        public async Task<IActionResult> Index(string qry)
        {
            var context = new EF.SampleDbContext();

            var res = await new BLL.Blog(context).Find(new EF.Blog { Title = qry, Body = qry });
            ViewBag.SearchResult = res.Where(x => x.DateInactive == null);

            return View(qry);
        }
    }
}
