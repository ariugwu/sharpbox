using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Audit;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;

namespace sharpbox
{
    public abstract class AppContext
    {
        #region Constructor(s)

        protected AppContext(string userIdentity, SmtpClient smtpClient, List<PublisherNames> extendedPublisherNames = null)
        {
            // Pub/Sub System(s)

            Dispatch = new Client(userIdentity, extendedPublisherNames ?? PublisherNames.DefaultPubList());
            Notification = new Notification.Client(Dispatch);

            // Module(s)

            Email = new Email.Client(smtpClient);
            Log = new Log.Client();

            var dispatcher = Dispatch;
            Audit = new Client<Package>(ref dispatcher); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

            File = new sharpbox.Io.Client();

        }

        #endregion

        protected AppContext() { }

        #region Encapsulated Entities

        // public Model.Domain.Environment.Info Environment { get; set; } Not sure environment is something to include
        public Dispatch.Client Dispatch { get; set; }
        public Notification.Client Notification { get; set; }

        #endregion

        #region Module(s)

        public Email.Client Email { get; set; }
        public Log.Client Log { get; set; }
        public Audit.Client<Package> Audit { get; set; }
        public Io.Client File { get; set; }

        #endregion

        #region Method(s)


        #endregion
    }
}
