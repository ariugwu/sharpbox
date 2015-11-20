using System;
using sharpbox.Common.Notification;

namespace sharpbox.Io.Model
{
    [Serializable]
    public class FileDetail : ITemplateType
    {
        public string FilePath { get; set; }
        public byte[] Data { get; set; }
    }
}
