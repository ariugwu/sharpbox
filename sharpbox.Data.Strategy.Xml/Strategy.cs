using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace sharpbox.Data.Strategy.Xml
{
    public class Strategy<T>
    {
        public Dictionary<string, object> Props { get; set; }
        public List<T> Entities { get; set; }

        public IEnumerable<T> All(Dispatch.Client dispatcher)
        {
            
        }

        public T Create(Dispatch.Client dispatcher, T entity,string filePath, T objectToWrite, bool append = false) where T : new()
    {
        TextWriter writer = null;
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            writer = new StreamWriter(filePath, append);
            serializer.Serialize(writer, objectToWrite);
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }
    }
  
        public T Get(Dispatch.Client dispatcher, int id);
        public T Update(Dispatch.Client dispatcher, T entity);
        public IEnumerable<T> UpdateAll(Dispatch.Client dispatcher, IEnumerable<T> list);
        public void Delete(Dispatch.Client dispatcher, T entity);
    }
}
