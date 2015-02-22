using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using sharpbox.Dispatch.Model;

namespace sharpbox.Email
{
    public class Client
    {
        private SmtpClient _smtpClient;

        public Client(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public void Send(List<string> to, string from, string subject, string body, List<string> cc, List<string> bcc, Dictionary<string, byte[]> attachments, bool isBodyHtml = true)
        {
            var mail = new MailMessage(from, string.Join(";", to))
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            };

            foreach (var ccAddress in cc)
            {
                mail.CC.Add(ccAddress);
            }

            foreach (var bccAddress in bcc)
            {
                mail.Bcc.Add(bccAddress);
            }

            foreach (var a in attachments)
            {
                // We do create new streams here and don't dispose them. This should be safe. @SEE: http://stackoverflow.com/a/2583991/2297580
                mail.Attachments.Add(new Attachment(new MemoryStream(a.Value), a.Key));
            }

            _smtpClient.Send(mail);
        }

        public void Send(List<string> to, string from, string subject, string body)
        {
            Send(to, from, subject, body, new List<string>(), new List<string>(), new Dictionary<string, byte[]>());
        }

    }
}
