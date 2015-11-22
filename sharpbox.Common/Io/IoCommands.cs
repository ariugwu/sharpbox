namespace sharpbox.Common.Io
{
    using sharpbox.Common.Dispatch.Model;

    public class IoCommands
    {
        public static readonly CommandName FileCreate = new CommandName("FileCreate");
        public static readonly CommandName FileDelete = new CommandName("FileDelete");
        public static readonly CommandName FileAccess = new CommandName("FileAccess");
    }
}
