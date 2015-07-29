using System;
using Newtonsoft.Json;
using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Domain.Notification.Model
{
    [Serializable]
    public class Subscriber
    {
        public Subscriber(EventNames eventName, string userId)
        {
            EventName = eventName;
            UserId = userId;
        }

        public Subscriber()
        {
            
        }

        private string _serializedType;

        public int SubscriberId { get; set; }
        public EventNames EventName { get; set; }
        public Type Type { get; set; }
        public string UserId { get; set; }

        public Guid? ApplicationId { get; set; }

        /// <summary>
        /// Used only for EF. @SEE: http://stackoverflow.com/a/14785553
        /// </summary>
        public string SerializeEntityType
        {
          get
          {
            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }; // Prevent circular reference errors with EF objects and other one-to-many relationships
            return _serializedType ?? (_serializedType = JsonConvert.SerializeObject(this.Type, serializerSettings));
          }

          set
          {
            _serializedType = value;
          }
        }

        public void DeserializeEntityType()
        {
          if (string.IsNullOrEmpty(_serializedType)) return;
          var type = JsonConvert.DeserializeObject<System.Type>(_serializedType);
          Type = type;
        }
    }
}
