using sharpbox.Audit;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;
using sharpbox.Model.Domain.Dispatch;

namespace sharpbox
{
    public class AppContext
    {
        #region Constructor(s)

        public AppContext(string userIdentity)
        {
            // Pub/Sub System(s)

            Dispatch = new Client(userIdentity, PublicationNamesExtension.ExtendedPubList);
            Notification = new Notification.Client();

            // Module(s)

            Email = new Email.Client();
            Log = new Log.Client();

            var dispatcher = Dispatch;
            Audit = new Client<Package>(ref dispatcher); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

            File = new sharpbox.Io.Client();
        }

        #endregion

        public AppContext() { }

        #region Encapsulated Entities

        public Model.Domain.Environment.Info Environment { get; set; }
        public Dispatch.Client Dispatch { get; set; }
        public Notification.Client Notification { get; set; }

        #endregion

        #region Module(s)

        public Email.Client Email { get; set; }
        public Log.Client Log { get; set; }
        public Audit.Client<Package> Audit { get; set; }
        public Io.Client File { get; set; }
        public Membership.Provider MembershipProvider { get; set; }

        #endregion

        #region Method(s)


        #endregion
    }
}
