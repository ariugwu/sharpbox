namespace sharpbox.Notification.Dispatch
{
    using sharpbox.Dispatch.Model;

    public class NotificationCommands
    {
        public static readonly CommandName AddNotificationBackLogItem = new CommandName("AddNotificationBackLogItem");
        public static readonly CommandName SendNotification = new CommandName("SendNotification");
        public static readonly CommandName AddNotificationSubscriber = new CommandName("AddNotificationSubscriber");
    }
}
