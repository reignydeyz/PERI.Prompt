using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Main.Controllers
{
    [Area("Main")]
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : BLL.BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return Redirect("~/Admin");
            else
                return View();
        }

        public IActionResult Forbidden()
        {
            return new StatusCodeResult(403);
        }
    }
}
