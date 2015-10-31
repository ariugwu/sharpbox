using System;
using sharpbox.Common.Type;

namespace sharpbox.Common.Dispatch.Model
{
    [Serializable]
    public class QueryName : EnumPattern
    {
        public QueryName(string value)
            : base(value)
        {
            Name = value;
        }

        public QueryName() { }

        public int CommandNameId { get; set; }
        public string Name { get; set; }

        public Guid? ApplicationId { get; set; }
    }
}
