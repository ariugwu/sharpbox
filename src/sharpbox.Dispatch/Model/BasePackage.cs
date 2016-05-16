using System;

namespace sharpbox.Dispatch.Model
{
    public abstract class BasePackage
    {
        public object Entity { get; set; }

        public Type Type { get; set; }

        public ResponseTypes ResponseType { get; set; }
    }
}
