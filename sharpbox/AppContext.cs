using System;

namespace sharpbox
{
    public class AppContext
    {
        public AppContext()
        {
            
        }

        #region Stuff

        public Model.Domain.Environment.Info Environment { get; set; }

        #endregion


        #region Module(s)

        public Email.Client Email { get; set; }
        public Log.Client Log { get; set; }
        public Audit.Client Audit { get; set; }
        public Io.Client File { get; set; }
        public Membership.Provider MembershipProvider { get; set; }

        #endregion

    }
}
