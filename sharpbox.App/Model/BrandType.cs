using System;

namespace sharpbox.App.Model
{
    using Common.Type;

    [Serializable]
    public class BrandType : EnumPattern
    {
        public BrandType(string value)
          : base(value)
        {
            Name = value;
        }

        public BrandType()
        {

        }
        public Guid BrandTypeId { get; set; }

        public string Name { get; set; }

    }
}
