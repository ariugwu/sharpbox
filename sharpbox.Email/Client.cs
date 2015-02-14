using System.Collections.Generic;
using System.Net.Mail;

namespace sharpbox.Email
{
    public class Client
    {
        public void Send(SmtpClient smtpClient, string to, string from, string subject, string body,
            Dictionary<string, byte[]> attachments = null)
        {
            var mail = new MailMessage("you@yourcompany.com", "user@hotmail.com") {Subject = subject, Body = body};
            smtpClient.Send(mail);
        }
    }
}
