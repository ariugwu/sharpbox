using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sharpbox.Bootstrap.Models
{
    using sharpbox.Common.Data;

    public class ExampleChildElement : ISharpThing<ExampleChildElement>
    {
        public Guid SharpId { get; set; }

        public string Title { get; set; }

        public bool IsValid { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ARandomNumber { get; set; }

        public double SomeDoubleNumber { get; set; }
    }
}