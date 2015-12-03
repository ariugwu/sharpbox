using System;

namespace sharpbox.Dispatch.Model
{
    using Common.Dispatch.Model;

    using sharpbox.Common.Dispatch;

    [Serializable]
    public class Response : BasePackage, IResponse
    {
        private string _message;

        public Response(IRequest request, string message, ResponseTypes responseType)
        {
            Entity = request.Entity;
            Message = message;
            RequestId = request.RequestId;
            Request = request;
            ResponseType = responseType;
            CreatedDate = DateTime.Now;
        }

        public Response() 
        {
            CreatedDate = DateTime.Now;        
        }

        public int ResponseId { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega
        
        public string Message
        {
            get { return _message + " [Status: " + ResponseType + "]"; }
            set { _message = value; }
        }

        public int EventNameId { get; set; }

        public EventName EventName { get; set; }

        public int RequestId { get; set; }
        public IRequest Request { get; set; }
        public string UserId { get; set;}

        public DateTime CreatedDate { get; set; }

        public Guid? EnvironmentId { get; set; }
    }
}
