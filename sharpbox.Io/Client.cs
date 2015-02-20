using System;
using System.IO;
using sharpbox.Dispatch.Model;
using sharpbox.Io.Strategy;

namespace sharpbox.Io
{
    public class Client
    {

        public Client(IStrategy strategy)
        {
            _strategy = strategy;
        }

        private IStrategy _strategy;
 
        public void Write<T>(Dispatch.Client dispatcher, string filePath, T objectToWrite, bool append = false)
            where T : new()
        {
            filePath = FixPath(filePath);
            _strategy.Write(dispatcher, filePath, objectToWrite, append);
            dispatcher.Broadcast(new Package() { Message = "File Written", EventName = EventNames.OnFileCreate, UserId = dispatcher.CurrentUserId });

        }

        public T Read<T>(Dispatch.Client dispatcher, string filePath) where T : new()
        {
            filePath = FixPath(filePath);
            var result = _strategy.Read<T>(dispatcher, filePath);
            dispatcher.Broadcast(new Package() { Message = "File Read", EventName = EventNames.OnFileAccess, UserId = dispatcher.CurrentUserId });

            return result;
        }

        public void Delete<T>(Dispatch.Client dispatcher, string filePath) where T : new()
        {
            filePath = FixPath(filePath);
            _strategy.Delete<T>(dispatcher, filePath);
            dispatcher.Broadcast(new Package() { Message = "File Deleted", EventName = EventNames.OnFileDelete, UserId = dispatcher.CurrentUserId });
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
