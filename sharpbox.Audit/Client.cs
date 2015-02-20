using System.Collections.Generic;
using System.Linq;
using sharpbox.Audit.Model;
using sharpbox.Audit.Strategy;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit
{
    public class Client
    {

        private IStrategy _strategy;

        public List<Package> Trail { get { return _strategy.Trail; } } 

        public Client(ref Dispatch.Client dispatcher, IStrategy strategy, AuditLevel auditLevel = AuditLevel.Basic)
        {
            _strategy = strategy; //?? new BaseStrategy<T>(dispatcher, props ?? new Dictionary<string, object> { { "xmlPath", "AuditXmlRepository.xml" } });
            ConfigureAuditLevel(dispatcher, auditLevel);
        }

        public void Record(Dispatch.Client dispatcher, Package package)
        {
            _strategy.RecordDispatch(dispatcher,package);
            dispatcher.Broadcast(new Package() { EventName = EventNames.OnAuditRecord, Message = "Audit entry recorded. Please check the Audit.Trail for details.", Entity = package, Type = typeof(Package), PackageId = 0, UserId = dispatcher.CurrentUserId });
        }

        private void ConfigureAuditLevel(Dispatch.Client dispatcher, AuditLevel auditLevel = AuditLevel.Basic)
        {
            // Capture the actions as well as the events. An example audit log could say -> Action -> Resulting Event. Which could be an exception. so we want to see the actions that preceed it.
            foreach (var a in dispatcher.AvailableActions)
            {
                dispatcher.Register(a, _strategy.RecordDispatch);
            }

            List<EventNames> list;
            switch (auditLevel)
            {
                case AuditLevel.Basic:
                    // For the basic call we excude audit calls and data persistence since the first is needless/problematic (event reflection) and the later is noisy and better served by other monitors.
                    list = dispatcher.AvailableEvents.Where(x => !x.ToString().ToLower().Contains("onaudit") && !x.ToString().ToLower().Contains("ondata") && !x.ToString().ToLower().Contains("onfile")).ToList();
                    foreach (var p in list)
                    {
                        dispatcher.Listen(p, _strategy.RecordDispatch);
                    }
                    break;
                case AuditLevel.All:
                    // We exclude any of the audit publishers because we don't want to create a circular call.
                    list = dispatcher.AvailableEvents.Where(x => !x.ToString().ToLower().Contains("onaudit")).ToList();
                    foreach (var p in list)
                    {
                        dispatcher.Listen(p, _strategy.RecordDispatch);
                    }
                    break;
                case AuditLevel.None:
                    break;
            }
        }

    }
}
