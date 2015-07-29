using System;
using sharpbox.Util.Domain.Notification;

namespace sharpbox.Io.Model
{
    [Serializable]
    public class FileDetail : ITemplateType
    {
        public string FilePath { get; set; }
        public byte[] Data { get; set; }
    }
}
