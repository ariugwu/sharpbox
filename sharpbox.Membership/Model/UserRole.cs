using System;

namespace sharpbox.Membership.Model
{
    using Common.Type;

    [Serializable]
    public class UserRole : EnumPattern
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

        public Guid ApplicationId { get; set; }
    }
}
