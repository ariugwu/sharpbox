using System;
using sharpbox.Util.Enum;

namespace sharpbox.Localization.Model
{
    [Serializable]
    public class ResourceName : EnumPattern
    {
        public ResourceName(string value) : base(value)
        {
            Name = value;
        }

        public ResourceName() { }

        public int ResourceNameId { get; set; }
        public string Name { get; set; }
    }
}
