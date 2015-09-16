using System;

namespace sharpbox.Membership.Model
{
  public class UserUserRole
  {
    public int UserUserRoleId { get; set; }
    public string UserLogin { get; set; }
    public UserRoleName UserRoleName { get; set; }

    public Guid ApplicationId { get; set; }

    public int UserRoleNameId { get; set; }
  }
}
