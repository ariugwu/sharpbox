using System;
using System.IO;
using sharpbox.Dispatch.Model;

namespace sharpbox.Io
{
    public class Client
    {
        public static void Save(Dispatch.Client dispatcher, string filename, byte[] data)
        {

            WriteContents(filename, data);

            dispatcher.Publish(new Package(){ Message = "File saved", PublisherName = PublisherNames.OnFileAccess});
        }

        public static string[] ReadFileLines(string path)
        {
            return System.IO.File.ReadAllLines(path);
        }

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        private void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        public static byte[] Load(Dispatch.Client dispatcher, string filename)
        {

            var data = ReadContents(filename);

            dispatcher.Publish(new Package() { Message = "File saved", PublisherName = PublisherNames.OnFileAccess });

            return data;
        }

        public static void SaveAudit(Dispatch.Client dispatcher, string filename, byte[] data)
        {
            WriteContents(filename, data);

            dispatcher.Publish(new Package() { Message = "File saved", PublisherName = PublisherNames.OnAuditPersist });
        }

        public static byte[] LoadAudit(Dispatch.Client dispatcher, string filename)
        {
            var data = ReadContents(filename);

            dispatcher.Publish(new Package() { Message = "Audit File loaded.", PublisherName = PublisherNames.OnAuditLoad });

            return data;
        }

        private static byte[] ReadContents(string filename)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            byte[] data = null;
            if (path != null)
            {
                path = path.Replace(@"file:\", String.Empty);
                data = System.IO.File.ReadAllBytes(Path.Combine(path, filename));
            }
            else
            {
                throw new DirectoryNotFoundException("Could not find path.");
            }

            return data;
        }

        private static void WriteContents(string filename, byte[] data)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            if (path != null)
            {
                path = path.Replace(@"file:\", String.Empty);
                File.WriteAllBytes(Path.Combine(path, filename), data);
            }
            else
            {
                throw new DirectoryNotFoundException("Could not find path.");
            }
        }

        public static void Delete(Dispatch.Client dispatcher, string filename)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            if (path != null)
            {
                path = path.Replace(@"file:\", String.Empty);
                System.IO.File.Delete(Path.Combine(path, filename));
            }
            else
            {
                throw new DirectoryNotFoundException("Could not find path.");
            }

            dispatcher.Publish(new Package() { Message = "File saved", PublisherName = PublisherNames.OnFileDelete });
        }

        public static bool Exists(string filename)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            if (path != null)
            {
                path = path.Replace(@"file:\", String.Empty);
                bool exists = System.IO.File.Exists(Path.Combine(path, filename));

                return exists;
            }
            else
            {
                throw new DirectoryNotFoundException("Could not find path.");
            }

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
