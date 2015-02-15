using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;
using sharpbox.Model.Domain.Dispatch;

namespace sharpbox
{
    public class AppContext
    {
        public AppContext()
        {
            Dispatch = new Client(PublicationNamesExtension.ExtendedPubList);
            
        }

        #region Encapsulated Entities

        public Model.Domain.Environment.Info Environment { get; set; }
        public Dispatch.Client Dispatch { get; set; }

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
