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
        public async Task<IActionResult> Index()
        {
            var qry = Request.Query["qry"].ToString();

            var context = new EF.SampleDbContext();

            var res = await new BLL.Blog(context).Find(new EF.Blog { Title = qry, Body = qry });
            res = res.Where(x => x.DateInactive == null);
            int cnt = res.Count();

            var page = Convert.ToInt16(Request.Query["page"]);
            var pager = new Core.Pager(res.Count(), page == 0 ? 1 : page);

            res = res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            ViewBag.SearchResult = res.ToList();

            return View(new Tuple<Models.Search, Core.Pager, int>(new Models.Search { Query = qry ?? "" }, pager, cnt));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Models.Search model)
        {
            return Redirect("~/Search/?qry=" + model.Query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IndexPage([Bind(Prefix = "Item1")] Models.Search model)
        {
            return Redirect("~/Search/?qry=" + model.Query);
        }
    }
}
