using sharpbox.Common.Dispatch.Model;

namespace sharpbox.Io.Dispatch
{
    public class IoCommands
    {
        public static readonly CommandName FileCreate = new CommandName("FileCreate");
        public static readonly CommandName FileDelete = new CommandName("FileDelete");
        public static readonly CommandName FileAccess = new CommandName("FileAccess");
    }
}
