using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Domain.Dispatch
{
    public class NotificationEvents
    {
        public static readonly EventNames OnNotificationBacklogItemAdded = new EventNames("OnNotificationBacklogItemAdded");
        public static readonly EventNames OnNotificationNotify = new EventNames("OnNotificationNotify");
        public static readonly EventNames OnNotificationAddSubScriber = new EventNames("OnNotificationAddSubScriber");
    }
}
