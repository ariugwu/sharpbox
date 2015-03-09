using sharpbox.Dispatch.Model;

namespace sharpbox.Io.Dispatch
{
    public class IoCommands
    {
        public static readonly CommandNames FileCreate = new CommandNames("FileCreate");
        public static readonly CommandNames FileDelete = new CommandNames("FileDelete");
        public static readonly CommandNames FileAccess = new CommandNames("FileAccess");
    }
}
