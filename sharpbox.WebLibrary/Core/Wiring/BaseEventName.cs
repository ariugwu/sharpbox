namespace sharpbox.WebLibrary.Core.Wiring
{
    using sharpbox.Common.Dispatch.Model;

    public class BaseEventName
    {
        public static EventName OnGet = new EventName("OnGet");
        public static EventName OnAdd = new EventName("OnAdd");
        public static EventName OnUpdate = new EventName("OnUpdate");
        public static EventName OnUpdateAll = new EventName("OnUpdateAll");
        public static EventName OnRemove = new EventName("OnRemove");
        public static EventName OnFrameworkCommand = new EventName("OnFrameworkCommand");
    }
}
