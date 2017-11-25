using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Controllers
{
    public class SearchController : BLL.BaseTemplateController
    {
        // GET: /<controller>/
        [Route("Search/{qry}")]
        public IActionResult Index(string qry)
        {
            var settings = ViewBag.Settings as List<EF.Setting>;
            var settings_cse = settings.Where(x => x.Group == "Google Search");
            var settings_cse_apikey = settings_cse.First(x => x.Key == "ApiKey").Value;
            var settings_cse_cse_id = settings_cse.First(x => x.Key == "CseId").Value;

            var cse = new Ext.GoogleSearch.CSE
            {
                ApiKey = settings_cse_apikey,
                CseId = settings_cse_cse_id,
                Query = qry,
            };

            var s = cse.Search();

            var page = Convert.ToInt16(Request.Query["page"]);
            var pager = new Core.Pager(s.Count(), page == 0 ? 1 : page);

            s = s.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            return View(new Tuple<string, List<Google.Apis.Customsearch.v1.Data.Result>>(cse.Query, s.ToList()));
        }
    }
}
