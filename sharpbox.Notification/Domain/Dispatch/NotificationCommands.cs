using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Domain.Dispatch
{
    public class NotificationCommands
    {
        public static readonly CommandNames AddNotificationBackLogItem = new CommandNames("AddNotificationBackLogItem");
        public static readonly CommandNames SendNotification = new CommandNames("SendNotification");
        public static readonly CommandNames AddNotificationSubscriber = new CommandNames("AddNotificationSubscriber");
    }
}
