﻿namespace sharpbox.Dispatch.Model
{
    using System;

    [Serializable]
  public abstract class EnumPattern
  {
    protected string Value { get; set; }

    protected EnumPattern(string value)
    {
      this.Value = value;
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
