using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class Feedback
    {
        public CommandNames ActionName { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
    }
}
