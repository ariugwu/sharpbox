using System;

namespace sharpbox.Util.Enum
{
    [Serializable]
    public abstract class EnumPattern
    {

        protected string _value;

        protected EnumPattern(string value)
        {
            _value = value;
        }

        protected EnumPattern() { }

        public override string ToString()
        {
            return _value;
        }
    }
}
