using System;

namespace sharpbox.Util.Enum
{
  [Serializable]
  public abstract class EnumPattern
  {
    protected string Value { get; set; }

    protected EnumPattern(string value)
    {
      Value = value;
    }

    protected EnumPattern() { }

    public override string ToString()
    {
      return Value;
    }

    public override bool Equals(object obj)
    {
      return obj.ToString().ToLower().Trim() == Value.ToLower().Trim();
    }
    public override int GetHashCode()
    {
      return (Value != null ? Value.GetHashCode() : 0);
    }

  }
}
