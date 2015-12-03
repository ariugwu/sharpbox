using sharpbox.Common.Type;

namespace sharpbox.Common.Dispatch.Model
{
  public class EntityStateName : EnumPattern
  {
    public EntityStateName(string value)
      : base(value)
    {
      Name = value;
    }

    public int EntityStateId { get; set; }
    public string Name { get; set; }

  }
}
