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
using System.Net;

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
            MailboxAddress from = new MailboxAddress("FGW IGRE System", "igreaddmin@gmail.com");
            MailboxAddress to = new MailboxAddress("QA Coordinator", mail.To);

            var email = new MimeMessage();
            email.From.Add(from);
            email.To.Add(to);
            email.Subject = mail.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = mail.Body };
            System.Diagnostics.Debug.WriteLine(">>>>>>> Fill");
            SendEmail(email);
        }

        public void SendEmail(MimeMessage mail)
        {
            var smtp = new SmtpClient();
            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            smtp.Connect("smtp.gmail.com", 465, true);
            smtp.Authenticate("igreaddmin@gmail.com", "igre@123");
            System.Diagnostics.Debug.WriteLine(">>>>>>> Send");
            smtp.Send(mail);
            smtp.Disconnect(true);
            smtp.Dispose();
        }


    }
}