using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Portal.Helper
{
    public class MailManager
    {
        public void SendSingleMail(string mail, string namesurname, string subject, string mailTemplate)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.yandex.ru", Convert.ToInt32("587"));
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("drive@kaktusyazilim.com", "@KYDrive124");
                MailAddress from = new MailAddress("drive@kaktusyazilim.com", namesurname);
                MailAddress to = new MailAddress(mail, namesurname);
                MailMessage message = new MailMessage(from, to);
                message.Body = mailTemplate;
                message.IsBodyHtml = true;
                message.Subject = subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}