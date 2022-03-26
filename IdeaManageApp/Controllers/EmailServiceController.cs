using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using IdeaManageApp.Models;

namespace IdeaManageApp.Controllers
{
    public class EmailServiceController : Controller
    {
        private AppModel db = new AppModel();

        // GET: EmailService
        public ActionResult Index()
        {
            return View();
        }

        public void FillEmailAndSend(EmailModel mail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("Admin...@gmail.com"));
            email.To.Add(MailboxAddress.Parse(mail.To));
            email.Subject = mail.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = mail.Body };

            SendEmail(email);
        }

        public void SendEmail(MimeMessage mail)
        {
            var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("Username", "Password");
            smtp.Send(mail);
            smtp.Disconnect(true);
        }
    }
}