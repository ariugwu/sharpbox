using System;
using sharpbox.Common.Data;

namespace sharpbox.Bootstrap.Models
{
    public class TestThing : ExampleChildElement, ISharpThing<ExampleChildElement>
    {
        public Guid SharpId { get; set; }
    }
}