using System;
using sharpbox.Util.Enum;

namespace sharpbox.Localization.Model
{
    [Serializable]
    public class ResourceNames : EnumPattern
    {
        public ResourceNames(string value) : base(value)
        {
            Name = value;
        }

        public int ResourceNamesId { get; set; }
        public string Name { get; set; }
    }
}
