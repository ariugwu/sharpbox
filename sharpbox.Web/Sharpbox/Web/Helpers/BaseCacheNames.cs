namespace sharpbox.Web.Sharpbox.Web.Helpers
{
  public class BaseCacheNames : sharpbox.Util.Enum.EnumPattern
  {
    public static readonly BaseCacheNames DummyData = new BaseCacheNames("DummyData");
    public static readonly BaseCacheNames LocalUsers = new BaseCacheNames("LocalUsers");
    public static readonly BaseCacheNames CostCenters = new BaseCacheNames("CostCenters");
    public static readonly BaseCacheNames ActiveDirectoryUsers = new BaseCacheNames("ActiveDirectoryUsers");
    public static readonly BaseCacheNames Resources = new BaseCacheNames("Resources");
    public static readonly BaseCacheNames OnlyAppResources = new BaseCacheNames("OnlyAppResources");
    public static readonly BaseCacheNames AppAndGlobalResources = new BaseCacheNames("AppAndGlobalResources");

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
