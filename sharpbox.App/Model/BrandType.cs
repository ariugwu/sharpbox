using sharpbox.Common.Type;

namespace sharpbox.App.Model
{
  public class BrandType : EnumPattern
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
