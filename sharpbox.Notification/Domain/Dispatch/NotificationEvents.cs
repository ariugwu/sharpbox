﻿using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Domain.Dispatch
{
    public class NotificationEvents
    {
        public static readonly EventName OnNotificationBacklogItemAdded = new EventName("OnNotificationBacklogItemAdded");
        public static readonly EventName OnNotificationNotify = new EventName("OnNotificationNotify");
        public static readonly EventName OnNotificationAddSubScriber = new EventName("OnNotificationAddSubScriber");
    }
}
