using System;

namespace sharpbox.Common.Data
{
    public interface ISharpThing<T> where T : new()
    {
        Guid SharpId { get; set; }
    }
}
