
namespace sharpbox.Membership.Model
{
  public class UserRole : Util.Enum.EnumPattern
  {
    public UserRole(string value)
      : base(value)
    {
      Name = value;
    }

    public int UserRoleNameId { get; set; }
    public string Name { get; set; }

    public static UserRole Administrator = new UserRole("Administrator");
    public static UserRole User = new UserRole("User");
  }
}
