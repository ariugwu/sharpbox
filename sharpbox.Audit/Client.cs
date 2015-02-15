using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sharpbox.Audit.Model;
using sharpbox.Audit.Strategy;

namespace sharpbox.Audit
{
    public class Client
    {
        #region Field(s)

        private IStrategy _strategy;

        #endregion

        #region Properties

        #endregion

        #region Constructor(s)

        public Client(Dispatch.Client dispatcher, IStrategy strategy = null, AuditLevel auditLevel = AuditLevel.None)
        {
            _strategy = strategy ?? new BaseStrategy();
            ConfigureAuditLevel(dispatcher, auditLevel);
        }
        #endregion

        #region Helper(s)

        private void ConfigureAuditLevel(Dispatch.Client dispatcher, AuditLevel auditLevel)
        {
            switch (auditLevel)
            {
                case AuditLevel.Basic:
                    break;
                case AuditLevel.All:
                    foreach (var p in dispatcher.AvailablePublications)
                    {
                        dispatcher.Subscribe(p, );
                    }
                    break;
                case AuditLevel.None:
                    break;
            }
        }

        #endregion
    }
}
