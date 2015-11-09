﻿namespace sharpbox.Notification.Model
{
    using System;

    using Newtonsoft.Json;

    using Common.Dispatch.Model;

    using sharpbox.Common.Data;

    [Serializable]
    public class Subscriber : ISharpThing<Subscriber>
    {
        public Subscriber(EventName eventName, string userId)
        {
            this.SharpId = Guid.NewGuid();
            this.EventName = eventName;
            this.UserId = userId;
        }

        public Subscriber()
        {
            this.SharpId = Guid.NewGuid();
        }

        private string _serializedType;

        public Guid SharpId { get; set; }
        public int SubscriberId { get; set; }
        public EventName EventName { get; set; }
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
            return this._serializedType ?? (this._serializedType = JsonConvert.SerializeObject(this.Type, serializerSettings));
          }

          set
          {
            this._serializedType = value;
          }
        }

        public void DeserializeEntityType()
        {
          if (string.IsNullOrEmpty(this._serializedType)) return;
          var type = JsonConvert.DeserializeObject<System.Type>(this._serializedType);
          this.Type = type;
        }
    }
}
