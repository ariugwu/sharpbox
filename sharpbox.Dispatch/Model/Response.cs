using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class Response
    {
        public Response(Request request, string message, ResponseTypes responseType)
        {
            Entity = request.Entity;
            Message = message;
            ResponseId = Guid.NewGuid();
            RequestId = request.RequestId;
            ResponseType = responseType;
        }

        public Response() { }

        public Guid ResponseId { get; set; }
        public int ResponseClusteringKey { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega
        public string Message { get; set; }
        public EventNames EventName { get; set; }
        public object Entity { get; set; }
        public Type Type { get; set; }

        public ResponseTypes ResponseType { get; set; }

        public Guid RequestId { get; set; }
    }
}
