using System;

namespace sharpbox.Io.Strategy.Excel
{
    [Serializable]
    public class ExcelStrategy : IStrategy
    {
        public void Write<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            throw new NotImplementedException();
        }

        public T Read<T>(string filePath) where T : new()
        {
            throw new NotImplementedException();
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
