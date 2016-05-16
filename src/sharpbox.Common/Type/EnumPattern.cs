using System;

namespace sharpbox.Common.Type
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
      return this.Value;
    }

    public override bool Equals(object obj)
    {
      return this.Value != null &&  (obj.ToString().ToLower().Trim() == this.Value.ToLower().Trim());
    }

    public override int GetHashCode()
    {
      return this.Value != null ? this.Value.GetHashCode() : 0;
    }

  }
}
