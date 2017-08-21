using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserController : BLL.BaseController
    {
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var context = new EF.SampleDbContext();
            ViewData["Title"] = "Users";
            ViewBag.Data = await new BLL.User(context).Find(new EF.User());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EF.User args)
        {
            var context = new EF.SampleDbContext();
            ViewData["Title"] = "Users";
            ViewBag.Data = await new BLL.User(context).Find(args);
            return View();
        }

        public IActionResult New()
        {
            ViewData["Title"] = "Users/New";
            var obj = new Tuple<EF.User, bool>(new EF.User(), true);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.User args, [Bind(Prefix = "Item2")] bool isactive)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var context = new EF.SampleDbContext();

                // Add user
                args.DateCreated = DateTime.Now;

                if (!isactive)
                    args.DateInactive = DateTime.Now;

                await new BLL.User(context).Add(args);
                
                return Redirect("~/Admin/User");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Entry is causing conflict or already exists");
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var context = new EF.SampleDbContext();
            ViewData["Title"] = "User/Edit";
            var rec = await new BLL.User(context).Get(new EF.User { UserId = id });
            return View(new Tuple<EF.User, bool>(rec, rec.DateInactive == null));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind(Prefix = "Item1")] EF.User args, [Bind(Prefix = "Item2")] bool isactive)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var context = new EF.SampleDbContext();

                // Edit user
                if (!isactive)
                    args.DateInactive = DateTime.Now;

                await new BLL.User(context).Edit(args);

                return Redirect("~/Admin/User");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Entry is causing conflict or already exists");
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var context = new EF.SampleDbContext();

            var buser = new BLL.User(context);
            
            await buser.Delete(ids);

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Activate([FromBody] int[] ids)
        {
            var context = new EF.SampleDbContext();

            var buser = new BLL.User(context);

            await buser.Activate(ids);

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate([FromBody] int[] ids)
        {
            var context = new EF.SampleDbContext();

            var buser = new BLL.User(context);

            await buser.Deactivate(ids);

            return Json("Success!");
        }
    }
}
