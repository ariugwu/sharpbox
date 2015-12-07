using System;
using sharpbox.Common.Type;

namespace sharpbox.Common.Dispatch.Model
{
    [Serializable]
    public class CommandName : EnumPattern
    {
        public CommandName(string value)
            : base(value)
        {
            this.CommandNameId = Guid.NewGuid();
            this.Name = value;
        }

        public CommandName()
        {
            this.CommandNameId = Guid.NewGuid();
        }

        public Guid CommandNameId { get; set; }
        public string Name { get; set; }

        public Guid? EnvironmentId { get; set; }
    }
}
