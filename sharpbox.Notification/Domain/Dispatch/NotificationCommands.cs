using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Domain.Dispatch
{
    public class NotificationCommands
    {
        public static readonly CommandName AddNotificationBackLogItem = new CommandName("AddNotificationBackLogItem");
        public static readonly CommandName SendNotification = new CommandName("SendNotification");
        public static readonly CommandName AddNotificationSubscriber = new CommandName("AddNotificationSubscriber");
    }
}
