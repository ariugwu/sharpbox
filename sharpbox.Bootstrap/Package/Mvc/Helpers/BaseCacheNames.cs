namespace Toolbox.Helpers
{
  public class BaseCacheNames
  {
    public static readonly BaseCacheNames DummyData = new BaseCacheNames("DummyData");

    public override string ToString()
    {
      return Value;
    }

    protected BaseCacheNames(string value)
    {
      this.Value = value;
    }

    public string Value { get; private set; }
  }
}
