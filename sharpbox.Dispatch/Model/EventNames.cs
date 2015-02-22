using System.Collections.Generic;

namespace sharpbox.Dispatch.Model
{
    /// <summary>
    /// As a rule there are events and requests. "Events" begin with "On" and have a One-To-Many relationship. Anything else is a request and should generally have a One-to-One relationship
    /// </summary>
    public class EventNames
    {


        public static readonly EventNames OnAuditRecord = new EventNames("OnAuditRecord");
        public static readonly EventNames OnAuditPersist = new EventNames("OnAuditPersist");
        public static readonly EventNames OnAuditLoad = new EventNames("OnAuditLoad");
        public static readonly EventNames OnRecordAction = new EventNames("OnRecordAction");

        public static readonly EventNames OnEmailSend = new EventNames("OnEmailSend");

        public static readonly EventNames OnFileCreate = new EventNames("OnFileCreate");
        public static readonly EventNames OnFileDelete = new EventNames("OnFileDelete");
        public static readonly EventNames OnFileAccess = new EventNames("OnFileAccess");

        public static readonly EventNames OnLogTrace = new EventNames("OnLogTrace");
        public static readonly EventNames OnLogInfo = new EventNames("OnLogInfo");
        public static readonly EventNames OnLogWarning = new EventNames("OnLogWarning");
        public static readonly EventNames OnLogException = new EventNames("OnLogException");

        public static readonly EventNames OnDataCreate = new EventNames("OnDataCreate");
        public static readonly EventNames OnDataUpdate = new EventNames("OnDataUpdate");
        public static readonly EventNames OnDataDelete = new EventNames("OnDataDelete");
        public static readonly EventNames OnDataGetById = new EventNames("OnDataGetById");
        public static readonly EventNames OnDataGetAll = new EventNames("OnDataGetAll");

        public static readonly EventNames OnUserChange = new EventNames("OnUserChange");

        public static readonly EventNames OnNotificationAddQueueEntry = new EventNames("OnNotificationAddQueueEntry");
        public static readonly EventNames OnNotificationAddBacklogItem = new EventNames("OnNotificationAddBacklogItem");
        public static readonly EventNames OnNotificationSaveBacklog = new EventNames("OnNotificationSaveBacklog");
        public static readonly EventNames OnNotificationSaveQueue = new EventNames("OnNotificationSaveQueue");
        public static readonly EventNames OnNotificationNotify = new EventNames("OnNotificationNotify");

        public static readonly EventNames OnFeedbackSet = new EventNames("OnFeedbackSet");

        protected string _value;

        public EventNames(string value)
        {
            _value = value;
        }

        public EventNames() { }

        public override string ToString()
        {
            return _value;
        }

        public static List<EventNames> DefaultPubList()
        {
            return new List<EventNames>()
            {
                OnLogTrace,
                OnLogInfo,
                OnLogWarning,
                OnLogException,
                OnFileCreate,
                OnFileDelete,
                OnFileAccess,
                OnEmailSend,
                OnAuditRecord,
                OnUserChange,
                OnNotificationAddBacklogItem,
                OnNotificationAddQueueEntry,
                OnNotificationNotify,
                OnNotificationSaveBacklog,
                OnNotificationSaveQueue,
                OnRecordAction
            };
        } 

    }
}
