using System;
using sharpbox.Util.Enum;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandNames : EnumPattern
    {

        public static readonly CommandNames BroadcastCommandStream = new CommandNames("BroadcastCommandStream");

        public static readonly CommandNames BroadcastCommandStreamAfterError = new CommandNames("BroadcastCommandStreamAfterError");

        public static readonly CommandNames PersistAuditTrail = new CommandNames("PersistAuditTrail");

        public static readonly CommandNames PersistNotificationBackLog = new CommandNames("PersistNotificationBackLog");

        public CommandNames(string value)
            : base(value)
        {
        }

        public CommandNames() { }

        public int CommandNameId { get; set; }
        public string Value { get { return _value; } }
    }
}
