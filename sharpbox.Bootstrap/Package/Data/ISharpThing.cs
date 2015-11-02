using System;

namespace sharpbox.WebLibrary.Data
{
    public interface ISharpThing<T> where T : new()
    {
        Guid SharpId { get; set; }
    }
}
