﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public abstract class BasePackage
    {
        private string _serializedType;
        private string _serializedEntity;

        public object Entity { get; set; }

        public Type Type { get; set; }

        public ResponseTypes ResponseType { get; set; }

        /// <summary>
        /// Used only for EF. @SEE: http://stackoverflow.com/a/14785553
        /// </summary>
        public string SerializedEntity
        {
            get
            {
                return _serializedEntity ?? (_serializedEntity = JsonConvert.SerializeObject(Entity));
            }

            set
            {
                _serializedEntity = value;
            }
        }

        /// <summary>
        /// Used only for EF. @SEE: http://stackoverflow.com/a/14785553
        /// </summary>
        public string SerializeEntityType
        {
            get
            {
                return _serializedType ?? (_serializedType = JsonConvert.SerializeObject(this.Type));
            }

            set
            {
                _serializedType = value;
            }
        }

        public void DeserializeEntity()
        {
            if (string.IsNullOrEmpty(_serializedEntity)) return;
            var entity = JsonConvert.DeserializeObject<object>(_serializedEntity);
            Entity = entity;
        }

        public void DeserializeEntityType()
        {
            if (string.IsNullOrEmpty(_serializedType)) return;
            var type = JsonConvert.DeserializeObject<System.Type>(_serializedType);
            Type = type;
        }


    }
}
