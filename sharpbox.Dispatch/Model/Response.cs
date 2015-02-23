using System;

namespace sharpbox.Dispatch.Model
{
    public class Response
    {
        public Response() { }

        public Guid ResponseId { get; set; }
        public int ResponseClusteringKey { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega
        public string Message { get; set; }
        public EventNames EventName { get; set; }
        public object Entity { get; set; }
        public Type Type { get; set; }
        public string UserId { get; set; }
    }
}
