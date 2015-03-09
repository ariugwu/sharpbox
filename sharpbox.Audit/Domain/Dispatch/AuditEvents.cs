using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Domain.Dispatch
{
    public class AuditEvents : EventNames
    {
        public static readonly EventNames OnAuditResponseAdded = new EventNames("OnAuditResponseAdded");
    }
}
