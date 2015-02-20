namespace sharpbox.Dispatch.Model
{
    public class Feedback
    {
        public ActionNames ActionName { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
    }
}
