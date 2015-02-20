using System;
namespace sharpbox.Dispatch.Model
{
    public class Request
    {
        public int RequestId { get; set; }
        public string Message { get; set; }
        public ActionNames ActionName { get; set; }
        public object Entity { get; set; }
        public Type Type { get; set; }
        public string UserId { get; set; }
    }
}
