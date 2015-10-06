using System;

namespace sharpbox.Membership.Model
{
  public class UserUserRole
  {
    public int UserUserRoleId { get; set; }
    public string LogOn { get; set; }
    public UserRole UserRole { get; set; }
    public Guid ApplicationId { get; set; }
    public int UserRoleId { get; set; }
  }
}
