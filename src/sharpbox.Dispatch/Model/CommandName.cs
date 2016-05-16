namespace sharpbox.Dispatch.Model
{
    using System;

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

    }
}
