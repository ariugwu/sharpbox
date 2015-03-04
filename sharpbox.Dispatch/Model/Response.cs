using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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

        public object Entity { get; set; }

        public Type Type { get; set; }

        public ResponseTypes ResponseType { get; set; }

        /// <summary>
        /// Used only for EF. @SEE: http://stackoverflow.com/a/14785553
        /// </summary>
        public string SerializedEntity
        {
            get { return JsonConvert.SerializeObject(Entity); }

            set
            {
                if(string.IsNullOrEmpty(value)) return;
                var entity = JsonConvert.DeserializeObject<object>(value);
                Entity = entity;
            }
        }

        /// <summary>
        /// Used only for EF. @SEE: http://stackoverflow.com/a/14785553
        /// </summary>
        public string SerializeEntityType
        {
            get
            {
                return JsonConvert.SerializeObject(this.Type);
            }

            set
            {
                if (string.IsNullOrEmpty(value)) return;
                var type = JsonConvert.DeserializeObject<System.Type>(value);
                Type = type;
            }
        }

        public Guid RequestId { get; set; }
        
    }
}
