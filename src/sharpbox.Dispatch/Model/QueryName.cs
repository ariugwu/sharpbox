﻿namespace sharpbox.Dispatch.Model
{
    using System;

    [Serializable]
    public class QueryName : EnumPattern
    {
        public QueryName(string value)
            : base(value)
        {
            this.QueryNameId = Guid.NewGuid();
            this.Name = value;
        }

        public QueryName()
        {
            this.QueryNameId = Guid.NewGuid();
        }

        public Guid QueryNameId { get; set; }
        public string Name { get; set; }

        public Guid? EnvironmentId { get; set; }
    }
}
