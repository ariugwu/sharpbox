using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class Response : BasePackage
    {
        public Response(Request request, string message, ResponseTypes responseType)
        {
            Entity = request.Entity;
            Message = message;
            ResponseUniqueKey = Guid.NewGuid();
            RequestId = request.RequestId;
            RequestUniqueKey = request.RequestUniqueKey;
            Request = request;
            ResponseType = responseType;
        }

        public Response() { }

        public int ResponseId { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega

        public Guid ResponseUniqueKey { get; set; }
        
        public string Message { get; set; }

        public int EventNameId { get; set; }

        public EventNames EventName { get; set; }

        public int RequestId { get; set; }
        public Guid RequestUniqueKey { get; set; }
        public Request Request { get; set; }
    }
}
