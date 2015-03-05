namespace sharpbox.Dispatch.Model
{
    public class ExtendedCommandNames : CommandNames
    {
        public static readonly CommandNames SendNotification = new CommandNames("SendNotification");
        public static readonly CommandNames AddNotificationSubscriber = new CommandNames("AddNotificationSubscriber");
        public static readonly CommandNames AddNotificationBackLogItem = new CommandNames("AddNotificationBackLogItem");
        public static readonly CommandNames SaveNotificationBackLogItem = new CommandNames("SaveNotificationBackLogItem");

        public static readonly CommandNames AddAuditResponse = new CommandNames("AddAuditResponse");
        public static readonly CommandNames SaveAuditResponse = new CommandNames("SaveAuditResponse");

        public static readonly CommandNames SendEmail = new CommandNames("SendEmail");

        public static readonly CommandNames FileCreate = new CommandNames("FileCreate");
        public static readonly CommandNames FileDelete = new CommandNames("FileDelete");
        public static readonly CommandNames FileAccess = new CommandNames("FileAccess");

        public static readonly CommandNames BroadcastCommandStream = new CommandNames("BroadcastCommandStream");

        public static readonly CommandNames BroadcastCommandStreamAfterError = new CommandNames("BroadcastCommandStreamAfterError");

    }
}
