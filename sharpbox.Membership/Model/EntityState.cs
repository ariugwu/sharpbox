using sharpbox.Common.Type;

namespace sharpbox.Membership.Model
{
  public class EntityState : EnumPattern
  {
    public EntityState(string value)
      : base(value)
    {
      Name = value;
    }

    public int UserRoleNameId { get; set; }
    public string Name { get; set; }

  }
}
