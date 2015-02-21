﻿using System.IO;
using System.Xml.Serialization;

namespace sharpbox.Io.Strategy.Xml
{

    public class XmlStrategy : sharpbox.Io.Strategy.IStrategy
    {

        public void Write<T>(Dispatch.Client dispatcher, string filePath, T objectToWrite, bool append = false) where T : new()
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

        public T Read<T>(Dispatch.Client dispatcher, string filePath) where T : new()
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

        public void Delete<T>(Dispatch.Client dispatcher, string filePath) where T : new()
        {
            throw new System.NotImplementedException();
        }

    }
}