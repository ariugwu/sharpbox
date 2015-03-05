namespace sharpbox.Dispatch.Model
{
    public class ExtendedEventNames : EventNames
    {
        public static readonly EventNames OnAllEvents = new EventNames("OnAllEvents");

        public static readonly EventNames OnBroadcastCommandStream = new EventNames("OnBroadcastCommandStream");
        public static readonly EventNames OnBroadcastCommandStreamAfterError = new EventNames("OnBroadcastCommandStreamAfterError");

        public static readonly EventNames OnAuditResponseAdded = new EventNames("OnAuditResponseAdded");
        public static readonly EventNames OnAuditResponseSaved = new EventNames("OnAuditResponseSaved");

        public static readonly EventNames OnEmailSend = new EventNames("OnEmailSend");

        public static readonly EventNames OnFileCreate = new EventNames("OnFileCreate");
        public static readonly EventNames OnFileDelete = new EventNames("OnFileDelete");
        public static readonly EventNames OnFileAccess = new EventNames("OnFileAccess");

        public static readonly EventNames OnNotificationBacklogItemAdded = new EventNames("OnNotificationBacklogItemAdded");
        public static readonly EventNames OnNotificationBacklogSaved = new EventNames("OnNotificationBacklogSaved");

        public static readonly EventNames OnNotificationNotify = new EventNames("OnNotificationNotify");
        public static readonly EventNames OnNotificationAddSubScriber = new EventNames("OnNotificationAddSubScriber");
        
    }
}
