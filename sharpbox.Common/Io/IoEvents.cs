namespace sharpbox.Common.Io
{
    using sharpbox.Common.Dispatch.Model;

    public class IoEvents
    {
        public static readonly EventName OnFileCreate = new EventName("OnFileCreate");
        public static readonly EventName OnFileDelete = new EventName("OnFileDelete");
        public static readonly EventName OnFileAccess = new EventName("OnFileAccess"); 
    }
}
