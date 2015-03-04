using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox
{
    public class BaseEventNames : EventNames
    {
        public static List<EventNames> BaseEventList()
        {
            return new List<EventNames>()
            {
                OnFileCreate,
                OnFileDelete,
                OnFileAccess,
                OnEmailSend,
                OnAuditTrailPersisted,
                OnNotificationNotify,
                OnNotificationBacklogPersisted,
                OnNotificationAddSubScriber,
                OnBroadcastCommandStream,
                OnBroadcastCommandStreamAfterError,
                OnException
            };
        }

        public static readonly EventNames OnBroadcastCommandStream = new EventNames("OnBroadcastCommandStream");
        public static readonly EventNames OnBroadcastCommandStreamAfterError = new EventNames("OnBroadcastCommandStreamAfterError");

        public static readonly EventNames OnAuditTrailPersisted = new EventNames("OnAuditTrailPersisted");

        public static readonly EventNames OnEmailSend = new EventNames("OnEmailSend");

        public static readonly EventNames OnFileCreate = new EventNames("OnFileCreate");
        public static readonly EventNames OnFileDelete = new EventNames("OnFileDelete");
        public static readonly EventNames OnFileAccess = new EventNames("OnFileAccess");

        public static readonly EventNames OnDataCreate = new EventNames("OnDataCreate");
        public static readonly EventNames OnDataUpdate = new EventNames("OnDataUpdate");
        public static readonly EventNames OnDataDelete = new EventNames("OnDataDelete");
        public static readonly EventNames OnDataGetById = new EventNames("OnDataGetById");
        public static readonly EventNames OnDataGetAll = new EventNames("OnDataGetAll");

        public static readonly EventNames OnNotificationBacklogPersisted = new EventNames("OnNotificationBacklogPersisted");
        public static readonly EventNames OnNotificationNotify = new EventNames("OnNotificationNotify");
        public static readonly EventNames OnNotificationAddSubScriber = new EventNames("OnNotificationAddSubScriber");
        
    }
}
