namespace sharpbox.Audit.Dispatch
{
    using sharpbox.Dispatch.Model;

    public class AuditEvents : EventName
    {
        public static readonly EventName OnAuditResponseAdded = new EventName("OnAuditResponseAdded");
    }
}
