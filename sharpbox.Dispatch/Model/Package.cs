using System;

namespace sharpbox.Dispatch.Model
{
    public class Package
    {
        public Package() { }

        public Guid PackageId { get; set; }
        public int PackageClusteringKey { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega
        public string Message { get; set; }
        public EventNames EventName { get; set; }
        public object Entity { get; set; }
        public Type Type { get; set; }
        public string UserId { get; set; }
    }
}
