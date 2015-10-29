using System.IO;
using System.Xml.Serialization;

namespace sharpbox.Io.Strategy.Xml
{

    public class XmlStrategy : IStrategy
    {

        public void Write<T>(string filePath, T objectToWrite, bool append = false) where T : new()
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

        public T Read<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public void Delete<T>(string filePath) where T : new()
        {
            throw new System.NotImplementedException();
        }

        public void Replace<T>(string originalFile, string newFile, string backupFile, T objectToWrite) where T : new()
        {
            throw new System.NotImplementedException();
        }
    }
}
