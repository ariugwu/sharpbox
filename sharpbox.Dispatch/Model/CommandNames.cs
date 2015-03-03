using System;
using System.Collections.Generic;
using sharpbox.Util.Enum;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandNames : EnumPattern
    {

        public static readonly CommandNames BroadcastCommandStream = new CommandNames("BroadcastCommandStream");

        public static readonly CommandNames BroadcastCommandStreamAfterError = new CommandNames("BroadcastCommandStreamAfterError");

        public CommandNames(string value)
            : base(value)
        {
        }

        public CommandNames() { }

        public static List<CommandNames> CommandList()
        {
            return new List<CommandNames>()
            {
                BroadcastCommandStream,
                BroadcastCommandStreamAfterError
            };
        }

    }
}
