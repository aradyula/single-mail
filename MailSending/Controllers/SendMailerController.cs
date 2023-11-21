using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MailSending.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MailSending.Controllers
{
    public class SendMailerController : Controller
    {
       
        public ActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SendMail(MailModel mail)
        {
            if (ModelState.IsValid)
            {
                bool isSend = false;
                try
                {
                    mail.Subject = !string.IsNullOrEmpty(mail.Subject) ? mail.Subject : "Enquiry Mail";
                    System.Globalization.TextInfo textinfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                    mail.Body = textinfo.ToTitleCase(mail.Body);
                    MailCredentails credentials = new MailCredentails();
                   
                    MailMessage mailMessage = new MailMessage(credentials.UserName, mail.To, mail.Subject, mail.Body);
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = credentials.Host;
                    smtp.Port = credentials.Port;
                    smtp.UseDefaultCredentials = false;
                    
                    smtp.Credentials = new System.Net.NetworkCredential(credentials.UserName, credentials.Password);
                    smtp.EnableSsl = true;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;
                    smtp.Send(mailMessage);
                    isSend = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (isSend == true)
                {
                    return Json(new { success = true, message = "Mail Sent Successfully..." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Mail Not Sent..." }, JsonRequestBehavior.AllowGet);
                }
            }
            return View(mail);
        }

    }
}