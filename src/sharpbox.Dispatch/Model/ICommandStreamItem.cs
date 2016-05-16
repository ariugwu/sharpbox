namespace sharpbox.Dispatch.Model
{
    public interface ICommandStreamItem
    {
        CommandName Command { get; set; }

        IResponse Response { get; set; }
    }
}
