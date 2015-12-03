using System;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.Dispatch.Model
{
    using sharpbox.Common.Dispatch;

    [Serializable]
    public class CommandStreamItem : ICommandStreamItem
    {
        public CommandName Command { get; set; }
        public IResponse Response { get; set; }
    }
}
