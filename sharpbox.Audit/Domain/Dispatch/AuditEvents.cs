using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Domain.Dispatch
{
    public class AuditEvents : EventName
    {
        public static readonly EventName OnAuditResponseAdded = new EventName("OnAuditResponseAdded");
    }
}
