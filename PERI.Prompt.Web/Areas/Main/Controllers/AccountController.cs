using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Main.Controllers
{
    [Area("Main")]
    [Authorize(Roles ="Admin,Blogger")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AccountController : BLL.BaseController
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult ChangePassword()
        {
            ViewData["Title"] = "Change Password";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(Models.ChangePassword args)
        {
            ViewData["Title"] = "Change Password";

            var context = new EF.SampleDbContext();

            if (!ModelState.IsValid)
                return View();

            var buser = new BLL.User(context);

            var user = await buser.Get(new EF.User { UserId = Convert.ToInt16(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.UserData).Value) });

            // Check password
            var salt = Core.Crypto.GenerateSalt();
            var enc = Core.Crypto.Hash(args.NewPassword, salt);
            var pwd = Core.Crypto.Hash(args.CurrentPassword, Convert.FromBase64String(user.PasswordSalt));
            if (pwd != user.PasswordHash)
            {
                ModelState.AddModelError(string.Empty, "Current password is invalid");
                return View();
            }

            user.PasswordHash = enc;
            user.PasswordSalt = Convert.ToBase64String(salt);
            user.LastPasswordChanged = DateTime.Now;
            user.DateConfirmed = DateTime.Now;
            user.ConfirmationCode = null;
            user.ConfirmationExpiry = null;
            user.DateInactive = null;

            await buser.Edit(user);

            TempData["notice"] = "Succesfully changed password.";
            return Redirect("~/Main");
        }
    }
}
