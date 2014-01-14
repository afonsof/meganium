using System.Net;
using System.Net.Mail;
using System.Text;

namespace Meganium.Api.Tools
{
    public class Mailer
    {
        public static void Send(string name, string email, string subject, string body, string to)
        {
            var smtpServer = Options.GlobalOptions.Get("SmtpServer");
            var smtpPort = Options.GlobalOptions.Get("SmtpPort", 587);
            var smtpUserName = Options.GlobalOptions.Get("SmtpUserName");
            var smtpPassword = Options.GlobalOptions.Get("SmtpPassword");
            var smtpUseSsl = Options.GlobalOptions.Get("SmtpUseSsl", true);

            var message = new MailMessage();
            message.To.Add(to);
            message.Subject = subject;
            message.From = new MailAddress(name + "<" + smtpUserName + ">");
            message.BodyEncoding = Encoding.UTF8;
            message.ReplyToList.Add(email);
            message.Body = body;

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
