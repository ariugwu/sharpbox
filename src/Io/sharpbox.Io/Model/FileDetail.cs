using System;

namespace sharpbox.Io.Model
{
    [Serializable]
    public class FileDetail
    {
        public string FilePath { get; set; }
        public byte[] Data { get; set; }
    }
}
