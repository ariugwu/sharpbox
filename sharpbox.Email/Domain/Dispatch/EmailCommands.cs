using sharpbox.Dispatch.Model;

namespace sharpbox.Email.Domain.Dispatch
{
    public class EmailCommands
    {
        public static readonly CommandName SendEmail = new CommandName("SendEmail");
    }
}
