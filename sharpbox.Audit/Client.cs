using System.Collections.Generic;
using System.Linq;
using sharpbox.Audit.Model;
using sharpbox.Audit.Strategy;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit
{
    public class Client
    {
        #region Field(s)

        private IStrategy _strategy;

        #endregion

        #region Properties

        public List<Package> Trail { get { return _strategy.Entries; } } 

        #endregion

        #region Constructor(s)

        public Client(Dispatch.Client dispatcher, IStrategy strategy = null, AuditLevel auditLevel = AuditLevel.None)
        {
            _strategy = strategy ?? new BaseStrategy();
            ConfigureAuditLevel(dispatcher, auditLevel);
        }
        #endregion

        #region Strategy Methods

        public void Record(Dispatch.Client dispatcher, Package package)
        {
            var result = _strategy.Create(package);
            dispatcher.Publish(new Package() { PublisherName = PublisherNames.OnAuditRecord, Message = "Audit entry recorded. Please check the Audit.Trail for details.", Entity = null, EntityType = this.GetType().FullName, PackageId = 0, UserId = "System" });
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
                    // We exclude any of the audit publishers because we don't want to create a circular call.
                    foreach (var p in dispatcher.AvailablePublications.Where(x => !x.ToString().ToLower().StartsWith("onaudit")))
                    {
                        dispatcher.Subscribe(p, _strategy.RecordDispatch);
                    }
                    break;
                case AuditLevel.None:
                    break;
            }
        }

        #endregion
    }
}
