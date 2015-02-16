using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch.Model;

namespace sharpbox.Email
{
    public class Client
    {
        public void Send(Dispatch.Client dispatcher, SmtpClient smtpClient, string to, string from, string subject, string body,
            Dictionary<string, byte[]> attachments = null)
        {
            var mail = new MailMessage("you@yourcompany.com", "user@hotmail.com") {Subject = subject, Body = body};
            smtpClient.Send(mail);

            dispatcher.Publish(new Package{ Entity = mail, Type = this.GetType(), Message = "E-Mail sent with message :" + subject , PublisherName = PublisherNames.OnEmailSend, PackageId = 0, UserId = ""});
        }

    }
}
