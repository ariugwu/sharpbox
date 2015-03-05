using System;
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
                if (string.IsNullOrEmpty(value)) return;
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
    }
}
