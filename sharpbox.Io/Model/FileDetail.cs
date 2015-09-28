using System;

namespace sharpbox.Io.Model
{
    using sharpbox.Util.Notification;

    [Serializable]
    public class FileDetail : ITemplateType
    {
        public string FilePath { get; set; }
        public byte[] Data { get; set; }
    }
}
