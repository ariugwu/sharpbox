using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox
{
    public class BaseCommandNames : CommandNames
    {
        public static List<CommandNames> BaseCommandList()
        {
            return new List<CommandNames>()
            {
                ChangeUser,
                BroadcastCommandStream,
                BroadcastEventStream,
                SendNotification,
                ProcessEmail,
                FileCreate,
                FileAccess,
                FileDelete,
                AddNotificationSubscriber
            };
        }

        public static readonly CommandNames BroadcastCommandStream = new CommandNames("BroadcastCommandStream");
        public static readonly CommandNames BroadcastEventStream = new CommandNames("BroadcastEventStream");

        public static readonly CommandNames SendNotification = new CommandNames("SendNotification");
        public static readonly CommandNames AddNotificationSubscriber = new CommandNames("AddNotificationSubscriber");

        public static readonly CommandNames ProcessEmail = new CommandNames("ProcessEmail");

        public static readonly CommandNames FileCreate = new CommandNames("FileCreate");
        public static readonly CommandNames FileDelete = new CommandNames("FileDelete");
        public static readonly CommandNames FileAccess = new CommandNames("FileAccess");

        public static readonly CommandNames ChangeUser = new CommandNames("ChangeUser");

    }
}
