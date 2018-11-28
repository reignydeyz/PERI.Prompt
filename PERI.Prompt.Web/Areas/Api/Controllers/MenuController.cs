using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PERI.Prompt.BLL;

namespace PERI.Prompt.Web.Areas.Api.Controllers
{
    [EnableCors("MyPolicy")]
    [Produces("application/json")]
    [Route("api/Menu")]
    public class MenuController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public MenuController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var res = from r in (await new BLL.Menu(unitOfWork).Find(new EF.Menu()))
                      select new
                      {
                          r.MenuId,
                          r.Name,
                          MenuItems = r.MenuItem.Count()
                      };

            return Json(res);
        }

        [HttpGet("[action]")]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await new BLL.Menu(unitOfWork).Get(new EF.Menu { MenuId = id });

            return Json(from r in res.MenuItem
                        select new {
                            r.MenuItemId,
                            r.Label,
                            r.Url
                        });
        }
    }
}