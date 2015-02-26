using System;
using System.Collections.Generic;
using sharpbox.Util.Enum;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandNames : EnumPattern
    {
        public CommandNames(string value) : base(value)
        {
        }

        public CommandNames() { }

        public static readonly CommandNames BroadcastCommandStream = new CommandNames("BroadcastCommandStream");
        public static readonly CommandNames BroadcastEventStream = new CommandNames("BroadcastEventStream");

        public static readonly CommandNames SetFeedback = new CommandNames("SetFeedback");

        public static readonly CommandNames SendNotification = new CommandNames("SendNotification");

        public static readonly CommandNames ChangeUser = new CommandNames("ChangeUser");

        public static List<CommandNames> DefaultActionList()
        {
            return new List<CommandNames>()
            {
                SetFeedback,
                ChangeUser,
                BroadcastCommandStream,
                BroadcastEventStream,
                SendNotification
            };
        }
    }
}
