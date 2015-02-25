using System;
using System.Collections.Generic;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandNames
    {
        public CommandNames(string value)
        {
            _value = value;
        }

        public CommandNames() { }

        public static readonly CommandNames BroadcastCommandStream = new CommandNames("BroadcastCommandStream");
        public static readonly CommandNames BroadcastEventStream = new CommandNames("BroadcastEventStream");

        public static readonly CommandNames SetFeedback = new CommandNames("SetFeedback");

        public static readonly CommandNames ChangeUser = new CommandNames("ChangeUser");

        protected string _value;

        public override string ToString()
        {
            return _value;
        }

        public static List<CommandNames> DefaultActionList()
        {
            return new List<CommandNames>()
            {
                SetFeedback,
                ChangeUser,
                BroadcastCommandStream,
                BroadcastEventStream
            };
        }
    }
}
