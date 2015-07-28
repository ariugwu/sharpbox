using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.Membership.Domain.Membership.Model
{
  public class UserRoleType : sharpbox.Util.Enum.EnumPattern
  {
    public UserRoleType(string value) : base(value)
    {
      
    }

    public static UserRoleType Administrator = new UserRoleType("Administrator");
    public static UserRoleType User = new UserRoleType("User");
  }
}
