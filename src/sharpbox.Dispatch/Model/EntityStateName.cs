namespace sharpbox.Dispatch.Model
{
    using System;

    public class EntityStateName : EnumPattern
    {
        public EntityStateName(string value)
          : base(value)
        {
            this.EntityStateId = Guid.NewGuid();
            this.Name = value;
        }

        public Guid EntityStateId { get; set; }
        public string Name { get; set; }

    }
}
