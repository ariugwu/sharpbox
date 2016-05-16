using System;
using sharpbox.Common.Type;

namespace sharpbox.Localization.Model
{
    [Serializable]
    public class ResourceName : EnumPattern
    {
        public ResourceName()
        {
            this.ResourceNameId = Guid.NewGuid();
        }

        public ResourceName(string value) : base(value)
        {
            this.ResourceNameId = Guid.NewGuid();
            this.Name = value;
        }

        public Guid ResourceNameId { get; set; }
        public string Name { get; set; }

        public Guid? EnvironmentId { get; set; }
    }
}
