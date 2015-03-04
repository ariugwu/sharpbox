using System.Collections.Generic;
using System.Linq;
using sharpbox.Audit.Model;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit
{
    public class Client
    {

        private List<Response> _trail; 

        /// <summary>
        /// The Event Stream from all users
        /// </summary>
        public List<Response> Trail { get { return _trail ?? (_trail = new List<Response>()); } set { _trail = value; } } 

        public Client(ref Dispatch.Client dispatcher,IEnumerable<EventNames> availableEvents, AuditLevel auditLevel = AuditLevel.Basic)
        {
            ConfigureAuditLevel(dispatcher, availableEvents, auditLevel);
        }

        public Client()
        {
            
        }

        public void Record(Response response)
        {
            Trail.Add(response);
        }

        private void ConfigureAuditLevel(Dispatch.Client dispatcher, IEnumerable<EventNames> availableEvents, AuditLevel auditLevel = AuditLevel.Basic)
        {
            List<EventNames> list;
            switch (auditLevel)
            {
                case AuditLevel.Basic:
                    // For the basic call we excude audit calls and data persistence since the first is needless/problematic (event reflection) and the later is noisy and better served by other monitors.
                    list = availableEvents.Where(x => !x.ToString().ToLower().Contains("onaudit") && !x.ToString().ToLower().Contains("ondata") && !x.ToString().ToLower().Contains("onfile")).ToList();
                    foreach (var p in list)
                    {
                        dispatcher.Listen(p, Record);
                    }
                    break;
                case AuditLevel.All:
                    // We exclude any of the audit publishers because we don't want to create a circular call.
                    list = availableEvents.Where(x => !x.ToString().ToLower().Contains("onaudit")).ToList();
                    foreach (var p in list)
                    {
                        dispatcher.Listen(p, Record);
                    }
                    break;
                case AuditLevel.None:
                    break;
            }
        }

    }
}
