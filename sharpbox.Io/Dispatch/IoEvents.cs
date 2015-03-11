using sharpbox.Dispatch.Model;

namespace sharpbox.Io.Dispatch
{
    public class IoEvents
    {
        public static readonly EventNames OnFileCreate = new EventNames("OnFileCreate");
        public static readonly EventNames OnFileDelete = new EventNames("OnFileDelete");
        public static readonly EventNames OnFileAccess = new EventNames("OnFileAccess"); 
    }
}
