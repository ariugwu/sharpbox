using System;

namespace sharpbox.Membership.Model
{
    [Serializable]
    public class UserUserRole
    {
        public int UserUserRoleId { get; set; }
        public string LogOn { get; set; }
        public UserRole UserRole { get; set; }
        public int UserRoleId { get; set; }
    }
}
