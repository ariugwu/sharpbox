using System;
namespace sharpbox.Dispatch.Model
{

    [Serializable]
    public class CommandStreamItem : ICommandStreamItem
    {
        public CommandName Command { get; set; }
        public IResponse Response { get; set; }
    }
}
