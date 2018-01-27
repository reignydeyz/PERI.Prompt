using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Controllers
{
    public class AuthorizationController : BLL.BaseController
    {
        /// <summary>
        /// This will signin the user
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task AddClaimsAndSignIn(EF.User args)
        {
            var ci = new ClaimsIdentity(
                    new[]
                    {
                        // User info
                        new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                        new Claim(ClaimTypes.Name, args.FirstName + " " + args.LastName),
                        new Claim(ClaimTypes.Email, args.Email),
                        new Claim(ClaimTypes.UserData, args.UserId.ToString()),
                        new Claim("MemberSince", Convert.ToDateTime(args.DateCreated).Year.ToString()),

                        // Role
                        new Claim(ClaimTypes.Role, args.Role.Name),
                    }, "MyCookieMiddlewareInstance");

            ClaimsPrincipal principal = new ClaimsPrincipal();
            principal.AddIdentity(ci);

            await HttpContext.SignInAsync("MyCookieMiddlewareInstance", principal);
        }

        [Route("SignIn")]
        public IActionResult SignIn()
        {
            ViewData["Title"] = "Sign in";
            return View();
        }

        [HttpPost]
        [Route("SignIn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(EF.User args)
        {
            var context = new EF.SampleDbContext();

            var buser = new BLL.User(context);

            var user = await buser.Get(new EF.User { Email = args.Email });

            if (user != null)
            {
                // Check if active
                if (user.DateInactive != null)
                {
                    ModelState.AddModelError(string.Empty, "Account is inactive.");
                    return View(args);
                }

                // Check password
                var salt = user.PasswordSalt;
                var saltBytes = Convert.FromBase64String(salt);

                if (Core.Crypto.Hash(args.PasswordHash, saltBytes) == user.PasswordHash)
                {
                    // Successful log in
                    user.LastSessionId = Guid.NewGuid().ToString();
                    user.LastLoginDate = DateTime.Now;
                    await buser.Edit(user);

                    await AddClaimsAndSignIn(user);

                    if (user.RoleId == (int)BLL.User.Roles.User)
                        return Redirect("~/Main");
                    else
                        return Redirect("~/Admin");
                }
            }

            ModelState.AddModelError(string.Empty, "Access denied.");
            return View(args);
        }

        [Route("SignUp")]
        public IActionResult SignUp()
        {
            ViewData["Title"] = "Sign up";

            var config = (ViewBag.Settings as List<EF.Setting>).Where(x => x.Group == "Config");

            if (config.First(x => x.Key == "Allow signup").Value == "0")
            {
                TempData["notice"] = "Signup is not allowed.";
                return Redirect("~/");
            }

            return View();
        }

        [HttpPost]
        [Route("SignUp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(EF.User args)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var context = new EF.SampleDbContext();

                // Add user
                args.DateCreated = DateTime.Now;
                await new BLL.User(context).Add(args);
                                
                var smtpSettings = (ViewBag.Settings as List<EF.Setting>).Where(x => x.Group == "Smtp");
                var smtpDisplayName = smtpSettings.First(x => x.Key == "Smtp display name").Value;
                var smtpDisplayEmail = smtpSettings.First(x => x.Key == "Smtp display email").Value;
                var smtpServer = smtpSettings.First(x => x.Key == "Smtp server").Value;
                var smtpPort = smtpSettings.First(x => x.Key == "Smtp port").Value;
                var smtpUseSsl = smtpSettings.First(x => x.Key == "Smtp use ssl").Value;
                var smtpUser = smtpSettings.First(x => x.Key == "Smtp user").Value;
                var smtpPwd = smtpSettings.First(x => x.Key == "Smtp password").Value;

                // Send email re: password
                var msg = new MimeMessage();

                msg.From.Add(new MailboxAddress(smtpDisplayName, smtpDisplayEmail));
                msg.To.Add(new MailboxAddress("", args.Email));
                msg.Subject = "Your password";
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "Click the link below to change your password:<br/>http://" + Request.Host.Value + "/authorization/confirmation/?userid=" + args.UserId + "&code=" + args.ConfirmationCode;
                msg.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, Convert.ToInt32(smtpPort), Convert.ToBoolean(smtpUseSsl));

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(smtpUser, smtpPwd);

                    client.Send(msg);
                    client.Disconnect(true);
                }

                TempData["notice"] = "Thank you. Please check your email for registration confirmation.";
                return Redirect("~/SignIn");
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
        [Route("SignUpWithPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpWithPassword(EF.User args)
        {
            try
            {
                var context = new EF.SampleDbContext();

                var configs = (ViewBag.Settings as List<EF.Setting>).Where(x => x.Group == "Config");
                var roleId = Convert.ToInt16(configs.First(x => x.Key == "Default RoleId").Value);
                var role = (await new BLL.Role(context).GetById(roleId)).Name;

                if (!ModelState.IsValid)
                    return View("SignUp", args);                

                // Asign role
                args.RoleId = roleId;
                args.Role = new EF.Role { RoleId = roleId, Name = role };

                // Add user
                args.DateCreated = DateTime.Now;
                await new BLL.User(context).Add(args);

                await AddClaimsAndSignIn(args);

                return RedirectToAction("Index", "Main");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Entry is causing conflict or already exists");
                return View("SignUp", args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("SignUpWithPasswordAndWithReCaptcha")]
        [ServiceFilter(typeof(BLL.ValidateReCaptchaAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpWithPasswordAndWithReCaptcha(EF.User args)
        {
            return await SignUpWithPassword(args);
        }

        public IActionResult Confirmation()
        {
            var userId = Request.Query["userid"].ToString();
            var code = Request.Query["code"].ToString();

            using (var context = new EF.SampleDbContext())
            {
                var rec = context.User.FirstOrDefault(x => x.ConfirmationCode == code);

                if (rec != null)
                {
                    if (Convert.ToDateTime(rec.ConfirmationExpiry) <  DateTime.Now)
                        return NotFound();

                    return View(new Models.ChangePassword { UserId = rec.UserId });
                }
                else
                    return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmation(Models.ChangePassword args)
        {
            ModelState.Clear();

            var context = new EF.SampleDbContext();

            if (args.NewPassword != args.ReEnterNewPassword)
            {
                ModelState.AddModelError(string.Empty, "New password & re-enter new password must match.");
                return View(args);
            }
            else
            {                
                var salt = Core.Crypto.GenerateSalt();
                var enc = Core.Crypto.Hash(args.NewPassword, salt);

                var rec = await new BLL.User(context).Get(new EF.User { UserId = args.UserId });                
                rec.PasswordHash = enc;
                rec.PasswordSalt = Convert.ToBase64String(salt);
                rec.LastPasswordChanged = DateTime.Now;
                rec.DateConfirmed = DateTime.Now;
                rec.ConfirmationCode = null;
                rec.ConfirmationExpiry = null;
                rec.DateInactive = null;

                await new BLL.User(context).Edit(rec);

                await AddClaimsAndSignIn(rec);

                TempData["notice"] = "Successfully changed password";

                return RedirectToAction("Index", "Main");
            }
        }

        [Route("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("MyCookieMiddlewareInstance");

            return RedirectToAction("SignIn", "Authorization");
        }

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            ViewData["Title"] = "Forgot password";
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(BLL.ValidateReCaptchaAttribute))]
        public async Task<IActionResult> ForgotPassword(EF.User args)
        {
            if (!ModelState.IsValid)
                return View();

            var context = new EF.SampleDbContext();
            var user = await new BLL.User(context).Get(new EF.User { Email = args.Email });

            if (user == null || user.DateInactive != null)
            {
                ModelState.AddModelError(string.Empty, "Unable to validate your email");
                return View();
            }

            // Generate ConfirmationCode
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");

            user.ConfirmationCode = guidString;
            user.ConfirmationExpiry = DateTime.Now.AddHours(12);

            // Update confirmation
            await new BLL.User(context).Edit(user);

            var smtpSettings = (ViewBag.Settings as List<EF.Setting>).Where(x => x.Group == "Smtp");
            var smtpDisplayName = smtpSettings.First(x => x.Key == "Smtp display name").Value;
            var smtpDisplayEmail = smtpSettings.First(x => x.Key == "Smtp display email").Value;
            var smtpServer = smtpSettings.First(x => x.Key == "Smtp server").Value;
            var smtpPort = smtpSettings.First(x => x.Key == "Smtp port").Value;
            var smtpUseSsl = smtpSettings.First(x => x.Key == "Smtp use ssl").Value;
            var smtpUser = smtpSettings.First(x => x.Key == "Smtp user").Value;
            var smtpPwd = smtpSettings.First(x => x.Key == "Smtp password").Value;

            // Send email re: password
            var msg = new MimeMessage();

            msg.From.Add(new MailboxAddress(smtpDisplayName, smtpDisplayEmail));
            msg.To.Add(new MailboxAddress("", args.Email));
            msg.Subject = "Your password";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "Click the link below to change your password:<br/>http://" + Request.Host.Value + "/authorization/confirmation/?userid=" + user.UserId + "&code=" + user.ConfirmationCode;
            msg.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, Convert.ToInt32(smtpPort), Convert.ToBoolean(smtpUseSsl));

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(smtpUser, smtpPwd);

                await client.SendAsync(msg);
                await client.DisconnectAsync(true);
            }

            TempData["notice"] = "Thank you. Please check your email for confirmation.";
            return Redirect("~/SignIn");
        }
    }
}
