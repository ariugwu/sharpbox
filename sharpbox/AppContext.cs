using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Audit;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;

namespace sharpbox
{
    public abstract class AppContext
    {
        /// <summary>
        /// This constructor will do some wiring for you.
        /// </summary>
        /// <param name="userIdentity">Used to create a Dispatch instance which is then used to bootstrap Notification, Log, and Audit functionality.</param>
        /// <param name="smtpClient">Powers the email client.</param>
        /// <param name="extendedPublisherNames">Used to bootstrap Dispatch. this way things like Audit wire into whatever events are in a derived system as well as the default list. If not provided the default list will be used.</param>
        protected AppContext(string userIdentity, SmtpClient smtpClient, List<PublisherNames> extendedPublisherNames = null)
        {
            // Pub/Sub System(s)

            Dispatch = new Client(userIdentity, extendedPublisherNames ?? PublisherNames.DefaultPubList());
            Notification = new Notification.Client(Dispatch);

            // Module(s)

            Email = new Email.Client(smtpClient);
            Log = new Log.Client(Dispatch);

            var dispatcher = Dispatch;
            Audit = new Client<Package>(ref dispatcher); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

        }

        protected AppContext() { }

        public Dispatch.Client Dispatch { get; set; }
        public Notification.Client Notification { get; set; }

        public Email.Client Email { get; set; }
        public Log.Client Log { get; set; }
        public Audit.Client<Package> Audit { get; set; }
    }
}
