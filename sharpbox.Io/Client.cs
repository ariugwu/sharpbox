using System;
using System.IO;
using sharpbox.Io.Model;

namespace sharpbox.Io
{
    using sharpbox.Common.Io;

    [Serializable]
    public class Client
    {
        public string DataPath { get; set; }

        public Client(IStrategy strategy)
        {
            _strategy = strategy;
        }

        private IStrategy _strategy;

        public void Write(FileDetail fileDetail)
        {
            while (IsFileLocked(new FileInfo(fileDetail.FilePath))) { 
                // Just wait if file is in use. 
            }

            File.WriteAllBytes(fileDetail.FilePath, fileDetail.Data);
        }

        public void Write<T>(string filePath, T objectToWrite, bool append = false)
            where T : new()
        {
            while (IsFileLocked(new FileInfo(filePath)))
            {
                // Just wait if file is in use. 
            }

            filePath = FixPath(filePath);
            _strategy.Write(filePath, objectToWrite, append);
        }

        public void Replace<T>(string filePath, T objectToWrite) where T : new()
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var fileExt = Path.GetExtension(filePath);
            var directory = Path.GetDirectoryName(filePath);

            var originalFile = FixPath(filePath);
            var newFile = FixPath(Path.Combine(directory, string.Format("{0}-new.{1}", fileName, fileExt)));
            var backupFile = FixPath(Path.Combine(directory, string.Format("{0}-backup.{1}", fileName, fileExt)));

            while (IsFileLocked(new FileInfo(originalFile)))
            {
                // Just wait if file is in use. 
            }

            _strategy.Replace<T>(originalFile, newFile, backupFile, objectToWrite);

        }

        public T Read<T>(string filePath) where T : new()
        {

            while (IsFileLocked(new FileInfo(filePath)))
            {
                // Just wait if file is in use. 
            }

            filePath = FixPath(filePath);
            var result = _strategy.Read<T>(filePath);

            return result;
        }

        public void Delete<T>(string filePath) where T : new()
        {
            while (IsFileLocked(new FileInfo(filePath)))
            {
                // Just wait if file is in use. 
            }

            filePath = FixPath(filePath);
            _strategy.Delete<T>(filePath);
        }

        private static string FixPath(string filePath)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            if (path != null)
            {
                path = path.Replace(@"file:\", String.Empty);
                return Path.Combine(path, filePath);
            }
            else
            {
                throw new DirectoryNotFoundException("Could not find path.");
            }
        }

        public bool Exists(string filename)
        {

            return System.IO.File.Exists(filename);

        }

        /// <summary>
        /// Not a perfect solution to the "Is File locked?" aka "Don't return me until the write operatin is finished"...but there is no perfect solution @SEE: http://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use/937558#937558
        /// @SEE: http://stackoverflow.com/a/937558
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
