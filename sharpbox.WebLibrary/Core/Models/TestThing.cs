namespace sharpbox.WebLibrary.Core.Models
{
    using System;

    using sharpbox.Common.Data;

    public class TestThing : ExampleChildElement, ISharpThing<ExampleChildElement>
    {
        public Guid SharpId { get; set; }
    }
}