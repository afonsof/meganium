using System.Net;
using System.Net.Mail;
using System.Text;

namespace MegaSite.Api.Tools
{
    public class Mailer
    {
        public static void Send(string name, string email, string subject, string body)
        {
            var contactMail = Options.Instance.Get("AdminEmail");
            var smtpServer = Options.Instance.Get("SmtpServer");
            var smtpPort = Options.Instance.Get("SmtpPort", 587);
            var smtpUserName = Options.Instance.Get("SmtpUserName");
            var smtpPassword = Options.Instance.Get("SmtpPassword");
            var smtpUseSsl = Options.Instance.Get("SmtpUseSsl", true);

            var message = new MailMessage();
            message.To.Add(contactMail);
            message.Subject = subject;
            message.From = new MailAddress(name + "<" + smtpUserName + ">");
            message.BodyEncoding = Encoding.UTF8;
            message.ReplyToList.Add(email);

            var smtpClient = new SmtpClient(smtpServer, smtpPort);
            if (smtpUseSsl)
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
            }
            smtpClient.Send(message);
        }
    }
}
