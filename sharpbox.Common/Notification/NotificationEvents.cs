﻿namespace sharpbox.Common.Notification
{
    using sharpbox.Common.Dispatch.Model;

    public class NotificationEvents
    {
        public static readonly EventName OnNotificationBacklogItemAdded = new EventName("OnNotificationBacklogItemAdded");
        public static readonly EventName OnNotificationNotify = new EventName("OnNotificationNotify");
        public static readonly EventName OnNotificationAddSubScriber = new EventName("OnNotificationAddSubScriber");
    }
}