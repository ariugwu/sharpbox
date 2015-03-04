using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class Response
    {
        public Response(Request request, string message, ResponseTypes responseType)
        {
            Entity = request.Entity;
            Message = message;
            ResponseUniqueKey = Guid.NewGuid();
            RequestId = request.RequestId;
            ResponseType = responseType;
        }

        public Response() { }

        [Key]
        public int ResponseId { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega

        public Guid ResponseUniqueKey { get; set; }
        
        public string Message { get; set; }

        public int EventNameId { get; set; }

        public EventNames EventName { get; set; }

        [NotMapped]
        public object Entity { get; set; }
        [NotMapped]
        public Type Type { get; set; }

        public ResponseTypes ResponseType { get; set; }

        /// <summary>
        /// Used only for EF
        /// </summary>
        private string Status { get { return ResponseType.ToString(); }}

        public Guid RequestId { get; set; }
        
    }
}
