using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class Response : BasePackage, IResponse
    {
        private string _message;

        public Response(IRequest request, string message, ResponseTypes responseType)
        {
            this.ResponseId = Guid.NewGuid();
            this.Entity = request.Entity;
            this.Message = message;
            this.RequestId = request.RequestId;
            this.Request = request;
            this.ResponseType = responseType;
            this.CreatedDate = DateTime.Now;
        }

        public Response() 
        {
            this.CreatedDate = DateTime.Now; 
            this.ResponseId = Guid.NewGuid();
        }

        public Guid ResponseId { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega
        
        public string Message
        {
            get { return _message + " [Status: " + ResponseType + "]"; }
            set { _message = value; }
        }

        public int EventNameId { get; set; }

        public EventName EventName { get; set; }

        public Guid RequestId { get; set; }
        public IRequest Request { get; set; }
        public string UserId { get; set;}

        public DateTime CreatedDate { get; set; }

        public Guid? EnvironmentId { get; set; }
    }
}
