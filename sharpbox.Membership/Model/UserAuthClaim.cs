using System;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.Membership.Model
{
  public class UserAuthClaim
  {
    public Type EntityType { get; set; }
    public EntityStateName EntityState { get; set; }
    public CommandName CommandName { get; set; }
    public UserRole UserRole { get; set; }
  }
}
