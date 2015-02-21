using System;

namespace sharpbox.Dispatch.Model
{
    public class Package
    {
        public Package() { }

        public int PackageId { get; set; }
        public string Message { get; set; }
        public EventNames EventName { get; set; }
        public object Entity { get; set; }
        public Type Type { get; set; }
        public string UserId { get; set; }
    }
}
