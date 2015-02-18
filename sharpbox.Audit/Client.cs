using System.Collections.Generic;
using System.Linq;
using sharpbox.Audit.Model;
using sharpbox.Audit.Strategy;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit
{
    public class Client<T> where T : class
    {
        #region Field(s)

        private Audit.Strategy.IStrategy<T> _strategy;

        #endregion

        #region Properties

        public List<T> Trail { get { return _strategy.Entries; } } 

        #endregion

        #region Constructor(s)

        public Client(ref Dispatch.Client dispatcher, Audit.Strategy.IStrategy<T> strategy = null, Dictionary<string, object> props = null, AuditLevel auditLevel = AuditLevel.Basic)
        {
            _strategy = strategy ?? new BaseStrategy<T>(dispatcher, props ?? new Dictionary<string, object> { { "xmlPath", "AuditXmlRepository.xml" } });
            ConfigureAuditLevel(dispatcher, auditLevel);
        }
        #endregion

        #region Strategy Methods

        public void Record(Dispatch.Client dispatcher, T entity)
        {
            var result = _strategy.Create(dispatcher,entity);
            dispatcher.Publish(new Package() { PublisherName = PublisherNames.OnAuditRecord, Message = "Audit entry recorded. Please check the Audit.Trail for details.", Entity = result, Type = this.GetType(), PackageId = 0, UserId = dispatcher.CurrentUserId });
        }

        #endregion

        #region Helper(s)

        private void ConfigureAuditLevel(Dispatch.Client dispatcher, AuditLevel auditLevel = AuditLevel.Basic)
        {
            List<PublisherNames> list;
            switch (auditLevel)
            {
                case AuditLevel.Basic:
                    // For the basic call we excude audit calls and data persistence since the first is needless/problematic (event reflection) and the later is noisy and better served by other monitors.
                    list = dispatcher.AvailablePublications.Where(x => !x.ToString().ToLower().Contains("onaudit") && !x.ToString().ToLower().Contains("ondata")).ToList();
                    foreach (var p in list)
                    {
                        dispatcher.Subscribe(p, _strategy.RecordDispatch);
                    }
                    break;
                case AuditLevel.All:
                    // We exclude any of the audit publishers because we don't want to create a circular call.
                    list = dispatcher.AvailablePublications.Where(x => !x.ToString().ToLower().Contains("onaudit")).ToList();
                    foreach (var p in list)
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
