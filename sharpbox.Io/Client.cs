using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace sharpbox.Io
{
    public class Client
    {
        #region Field(s)
        #endregion

        #region Properties
        #endregion

        #region Constructor(s)
        #endregion

        #region Method(s)

        public static void Save(string filename, byte[] data)
        {
            var path = Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase );

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

        public static byte[] Load(string filename)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            if (path != null)
            {
                path = path.Replace(@"file:\", String.Empty);
                return System.IO.File.ReadAllBytes(Path.Combine(path, filename));
            }
            else
            {
                throw new DirectoryNotFoundException("Could not find path.");
            }
            
        }

        public static void Delete(string filename)
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
        #endregion
    }
}
