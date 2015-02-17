using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch.Model;

namespace sharpbox.Cli.Model.Domain.AppContext
{
    public class ConsoleContext : sharpbox.AppContext
    {
        public ConsoleContext(string userIdentity, SmtpClient smtpClient, List<PublisherNames> publisherNames)
            : base(userIdentity, smtpClient, publisherNames)
        {
            
        }
    }
}
