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
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("linhemma0601@gmail.com"));
            email.To.Add(MailboxAddress.Parse(mail.To));
            email.Subject = mail.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = mail.Body };
            System.Diagnostics.Debug.WriteLine(">>>>>>> Fill");
            SendEmail(email);
        }

        public void SendEmail(MimeMessage mail)
        {
            var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (sender, certificate, certChainType, errors) => true;
            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(new NetworkCredential("linhemma0601@gmail.com", "Emma0777026608"));
            //smtp.Authenticate("linhemma0601@gmail.com", "Emma0777026608");
            System.Diagnostics.Debug.WriteLine(">>>>>>> Send");
            smtp.Send(mail);
            smtp.Disconnect(true);
        }


    }
}