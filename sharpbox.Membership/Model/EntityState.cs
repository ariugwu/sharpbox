namespace sharpbox.Membership.Model
{
  public class EntityState : Util.Enum.EnumPattern
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
