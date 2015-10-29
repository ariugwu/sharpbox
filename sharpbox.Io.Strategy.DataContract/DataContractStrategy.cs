using System;
using System.IO;
using System.Runtime.Serialization;

namespace sharpbox.Io.Strategy.DataContract
{
    public class DataContractStrategy : IStrategy
    {
        public void Write<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var ser = new DataContractSerializer(typeof(T));
            }
        }

        public T Read<T>(string filePath) where T : new()
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        public void Delete<T>(string filePath) where T : new()
        {
            throw new NotImplementedException();
        }

        public void Replace<T>(string originalFile, string newFile, string backupFile, T objectToWrite) where T : new()
        {
            throw new NotImplementedException();
        }
    }
}
