using sharpbox.Localization.Model;

namespace sharpbox.Dispatch.Model
{
    public class Feedback
    {
        public Resource Warning { get; set; }
        public Resource Info { get; set; }
        public Resource Error { get; set; }
        public Resource Success { get; set; }
    }
}
