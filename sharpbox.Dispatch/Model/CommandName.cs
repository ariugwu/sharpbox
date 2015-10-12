using System;
using sharpbox.Common.Type;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandName : EnumPattern
    {
        public CommandName(string value)
            : base(value)
        {
            Name = value;
        }

        public CommandName() { }

        public int CommandNameId { get; set; }
        public string Name { get; set; }

        public Guid? ApplicationId { get; set; }
    }
}
