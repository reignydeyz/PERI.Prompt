using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Controllers
{
    public class HomeController : BLL.BaseTemplateController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Models.Contact args)
        {
            if (!ModelState.IsValid)
                return View("Index", args);

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

            msg.From.Add(new MailboxAddress(args.Name, args.Email));
            msg.To.Add(new MailboxAddress("", smtpUser));
            msg.Subject = args.Subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = args.Message;
            msg.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, Convert.ToInt32(smtpPort), Convert.ToBoolean(smtpUseSsl));

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(smtpUser, smtpPwd);

                await client.SendAsync(msg);
                await client.DisconnectAsync(true);
            }

            TempData["notice"] = "Thank you for contacting us. We`ll review your message and get back to you soon.";
            return Redirect("~/");
        }

        [HttpPost]
        [ServiceFilter(typeof(BLL.ValidateReCaptchaAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactWithReCaptcha(Models.Contact args)
        {
            return await Contact(args);
        }
    }
}
