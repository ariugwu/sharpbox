namespace sharpbox.WebLibrary.Core
{
  public class BrandType : Util.Enum.EnumPattern
  {
    public BrandType(string value)
      : base(value)
    {
      Name = value;
    }

    public BrandType()
    {

    }
    public int BrandTypeId { get; set; }

    public string Name { get; set; }

  }
}
