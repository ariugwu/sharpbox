using System;

namespace sharpbox.Membership.Model
{
  public class UserAuthClaim
  {
    public Type EntityType { get; set; }
    public EntityState EntityState { get; set; }
    public UserRole UserRole { get; set; }
  }
}
