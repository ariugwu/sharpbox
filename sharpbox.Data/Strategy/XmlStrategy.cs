using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace sharpbox.Data.Strategy
{
    public class XmlStrategy<T> : IStrategy<T>
    {
        #region Constructor(s)

        public XmlStrategy(Dispatch.Client dispatcher, Dictionary<string, object> auxInfo)
        {
            AuxInfo = auxInfo;
            Init(dispatcher);
        } 

        #endregion
        
        private List<T> _entities;
 
        public Dictionary<string, object> AuxInfo { get; set; }

        public List<T> Entities
        {
            get
            {
                return _entities;
            } 
            set { _entities = value; }
        }

        private string _xmlPath;
        private XmlSerializer _serializer = new XmlSerializer(typeof(List<T>));

        #region Interface Methods / Members
        public void Init(Dispatch.Client dispatcher)
        {
            _xmlPath = (string)AuxInfo["xmlPath"];
            Load(dispatcher);
        }

        public IEnumerable<T> All(Dispatch.Client dispatcher)
        {

            return Entities;
        }

        public T Create(Dispatch.Client dispatcher, T entity)
        {

            Entities.Add(entity);
            Save(dispatcher);

            return entity;
        }

        public T Get(Dispatch.Client dispatcher, int id)
        {
            throw new System.NotImplementedException();
        }

        public T Update(Dispatch.Client dispatcher, T entity)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> UpdateAll(Dispatch.Client dispatcher, IEnumerable<T> list)
        {
            Entities = list.ToList();
            Save(dispatcher);
            return Entities;
        }

        public void Delete(Dispatch.Client dispatcher, T entity)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Helper(s)

        public void Load(Dispatch.Client dispatcher)
        {
            if (!sharpbox.Io.Client.Exists(_xmlPath))
            {
                Entities = new List<T>();
                Save(dispatcher);
                return;
            }

            var data = sharpbox.Io.Client.LoadAudit(dispatcher, _xmlPath);
            using (var ms = new MemoryStream(data))
            {
                Entities = (List<T>)_serializer.Deserialize(ms);

            }
        }

        public void Save(Dispatch.Client dispatcher)
        {
            using (var ms = new MemoryStream())
            {
                _serializer.Serialize(ms, Entities);
                sharpbox.Io.Client.SaveAudit(dispatcher, _xmlPath, ms.ToArray());
            }

        }

        #endregion
    }
}
