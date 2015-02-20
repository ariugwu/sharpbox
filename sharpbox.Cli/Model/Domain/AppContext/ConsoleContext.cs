using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch.Model;

namespace sharpbox.Cli.Model.Domain.AppContext
{
    public class ConsoleContext : sharpbox.AppContext
    {
        /// <summary>
        /// Extension of the AppContext which contains the dispatcher. All we've done is throw in some dispatcher friendly components.
        /// </summary>
        /// <param name="userIdentity">Used to create a Dispatch instance which is then used to bootstrap Notification, Log, and Audit functionality.</param>
        /// <param name="publisherNames">Used to bootstrap Dispatch. this way things like Audit wire into whatever events are in a derived system as well as the default list. If not provided then at empty list is used.</param>
        /// <param name="smtpClient">Powers the email client.</param>
        public ConsoleContext(string userIdentity, List<PublisherNames> publisherNames, SmtpClient smtpClient)
            : base(userIdentity, publisherNames)
        {
            Notification = new Notification.Client(Dispatch);
            Email = new Email.Client(smtpClient);
            Log = new Log.Client(Dispatch);
            var dispatcher = Dispatch;
            Audit = new Audit.Client<Package>(ref dispatcher); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.
        }

        public Notification.Client Notification { get; set; } // A dispatch friendly notification system.
        public Email.Client Email { get; set; } // A dispatch friendly email client
        public Log.Client Log { get; set; } // A dispatch friendly logger
        public Audit.Client<Package> Audit { get; set; } // A dispatch friendly Auditor
    }
}
