﻿using System;
using System.IO;

namespace sharpbox.Io.Strategy.Binary
{
    using Common.Io;

    [Serializable]
    public class BinaryStrategy : IStrategy
    {
        public void Write<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
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
            this.Write(newFile, objectToWrite);

            File.Replace(newFile, originalFile, backupFile);
        }
    }
}
