
namespace sharpbox.Membership.Model
{
  public class UserRoleName : Util.Enum.EnumPattern
  {
    public UserRoleName(string value)
      : base(value)
    {
      Name = value;
    }

    public int UserRoleNameId { get; set; }
    public string Name { get; set; }

    public static UserRoleName Administrator = new UserRoleName("Administrator");
    public static UserRoleName User = new UserRoleName("User");
  }
}
